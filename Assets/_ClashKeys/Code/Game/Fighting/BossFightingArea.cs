using System;
using System.Collections.Generic;
using System.Linq;
using ClashKeys.Game.PlayerComponents;
using Game.AssetContent;
using Game.Extensions;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace ClashKeys.Game.Fighting
{
internal class BossFightingArea
{
    private readonly Player _player;
    private readonly IObjectResolver _resolver;
    private readonly BulletController _bulletController;
    private readonly IResourceFactoryManager _resourceFactory;
    private readonly List<Enemy> _enemies = new(4);
    private EnemyFightView _mapView;

    public event Action<Enemy> OnEnemyDead;
    public bool HasAliveEnemy => _enemies.Count > 0;
    public Vector3 ChestPosition => _mapView.chestTransform.position;

    public BossFightingArea(Player player, Vector3 position, IObjectResolver resolver)
    {
        _player = player;
        _resolver = resolver;
        _resourceFactory = resolver.Resolve<IResourceFactoryManager>();
        _bulletController = resolver.Resolve<BulletController>();

        GenerateMap(position);
    }

    private void GenerateMap(Vector3 position)
    {
        _mapView = _resourceFactory.CreateGameObjectWithComponent<EnemyFightView>("FightView", position, Quaternion.identity, null);
        var enemy1 = new Enemy(_resolver, _mapView.enemyPos1);
        var enemy2 = new Enemy(_resolver, _mapView.enemyPos2);

        _enemies.Add(enemy1);
        _enemies.Add(enemy2);

        foreach (var enemy in _enemies)
            enemy.View.OnEntered += OnEnterToEnemy;
    }

    private void OnEnterToEnemy(EnemyView enemyView, GameObject collided)
    {
        if (collided.TryGetComponent<IBulletSource>(out var bullet) == false)
            return;

        if (bullet.Source == enemyView.transform)
            return;

        _bulletController.Remove(bullet);

        var enemy = _enemies.First(x => x.View == enemyView);
        ProcessReceiveBulletDamage(enemy);
    }

    private void ProcessReceiveBulletDamage(Enemy enemy)
    {
        enemy.Hp--;

        if (enemy.Hp != 0)
            return;

        _enemies.Remove(enemy);
        enemy.DisposeWeapon();
        Object.Destroy(enemy.View.gameObject);
        OnEnemyDead?.Invoke(enemy);
    }

    public Transform GetFreeAliveEnemy()
    {
        var enemy = _enemies.Random();

        return enemy.View.transform;
    }

    public void StartAttack() => _enemies.ForEach(x => x.AttachWeapon());

    public void StopAttack() => _enemies.ForEach(x => x.DisposeWeapon());

    public void AttackPlayer(Vector3 position) => _enemies.ForEach(x => x.AttackPlayer(position));
}
}