using System;
using ClashKeys.Game.Fighting;
using ClashKeys.Game.PlayerComponents;
using ClashKeys.UI;
using Game.AssetContent;
using Game.FSMCore;
using Game.FSMCore.States;
using Game.Pools;
using UnityEngine;
using VContainer;

namespace ClashKeys.Game.Core.States
{
internal class EnemyFightingStateArgs
{
    public Vector3 PivotPoint;
}

internal class EnemyFightingState : IActivatedState<EnemyFightingStateArgs>
{
    private readonly FSMCore _fsm;
    private readonly Player _player;
    private readonly WindowDirector _windowDirector;
    private readonly IObjectResolver _resolver;
    private readonly IObjectPoolManager _objectPoolManager;
    private readonly BulletController _bulletController;

    private PlayerFightingMover _mover;
    private BossFightingArea _enemyArea;

    public EnemyFightingState(FSMCore fsm,
                              Player player,
                              WindowDirector windowDirector,
                              IObjectResolver resolver,
                              IObjectPoolManager objectPoolManager,
                              BulletController bulletController)
    {
        _fsm = fsm;
        _player = player;
        _windowDirector = windowDirector;
        _resolver = resolver;
        _objectPoolManager = objectPoolManager;
        _bulletController = bulletController;
    }

    void IActivatedState<EnemyFightingStateArgs>.ActivateState(IStateMachine machine,
                                                               EnemyFightingStateArgs stateArgs)
    {
        var window = _windowDirector.OpenEnemyFightingWindow();
        window.OnClickAttack += ProcessAttackEnemy;

        _mover = new PlayerFightingMover(window, _player.transform, 3f, 0, 9);
        _enemyArea = new BossFightingArea(_player, stateArgs.PivotPoint, _resolver);
        _enemyArea.OnEnemyDead += OneOfEnemyDead;
        _player.OnEntered += OnPlayerEnterGameObject;

        _enemyArea.StartAttack();
        AttachWeaponToPlayer();
    }

    public void Finish()
    {
        var window = _windowDirector.GetWindow<EnemyFightingWindowMediatorUI>();
        window.OnClickAttack -= ProcessAttackEnemy;
        _windowDirector.CloseWindow<EnemyFightingWindowMediatorUI>();

        _player.OnEntered -= OnPlayerEnterGameObject;
        _player.SkipWeapon();
        _enemyArea.OnEnemyDead -= OneOfEnemyDead;

        _enemyArea.StopAttack();
        _mover.Dispose();
        _bulletController.Clear();
    }

    public void UpdateState()
    {
        _mover.UpdateMove(Time.deltaTime);
        _enemyArea.AttackPlayer(_player.transform.position);
    }

    private void OneOfEnemyDead(Enemy enemy)
    {
        if (_enemyArea.HasAliveEnemy)
            return;

        _fsm.Chest(new ChestStateArgs
        {
            chestPosition = _enemyArea.ChestPosition,
        });
    }

    private void OnPlayerEnterGameObject(GameObject collided)
    {
        if (collided.TryGetComponent<IBulletSource>(out var bullet) == false)
            return;

        if (bullet.Source == _player.transform)
            return;

        _bulletController.Remove(bullet);

        _fsm.Defeat(new DefeatStateArgs
        {
            Reason = "You were killed by an enemy"
        });
    }

    private void ProcessAttackEnemy()
    {
        var freeEnemy = _enemyArea.GetFreeAliveEnemy();

        if (_player.Weapon.TryShoot(freeEnemy.position, out var bullet) == false)
            return;

        bullet.Source = _player.transform;
    }

    private void AttachWeaponToPlayer()
    {
        var bulletPrefab = _resolver
                           .Resolve<IResourceManager>()
                           .LoadAsset<GameObject>("Bullet")
                           .GetComponent<BulletView>();

        var weapon = new SimpleWeapon<BulletView>(_player.transform, bulletPrefab, 1f, _objectPoolManager,
                                                  _bulletController);
        _player.AttackWeapon(weapon);
    }
}

internal class PlayerFightingMover : IDisposable
{
    private readonly float _moveSpeed;
    private readonly float _minX;
    private readonly float _maxX;
    private readonly Transform _player;
    private readonly EnemyFightingWindowMediatorUI _window;

    private bool IsMoving => _moveDirection != Vector2Int.zero;
    private Vector2Int _moveDirection;

    public PlayerFightingMover(EnemyFightingWindowMediatorUI window, Transform player, float moveSpeed, float minX,
                               float maxX)
    {
        _minX = minX;
        _maxX = maxX;
        _window = window;
        _player = player;
        _moveSpeed = moveSpeed;

        Initialize();
    }

    private void Initialize()
    {
        _window.OnClickMoveStart += StartMoving;
        _window.OnClickMoveEnd += StopMoving;
    }

    public void Dispose()
    {
        _window.OnClickMoveStart -= StartMoving;
        _window.OnClickMoveEnd -= StopMoving;
        _moveDirection = Vector2Int.zero;
    }

    public void UpdateMove(float deltaTime)
    {
        if (IsMoving == false)
            return;

        var direction = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
        var newPosition = _player.position + direction * _moveSpeed * deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);

        _player.position = newPosition;
    }

    private void StartMoving(Vector2Int direction)
    {
        if (IsMoving)
            return;

        _moveDirection = direction;
    }

    private void StopMoving(Vector2Int direction)
    {
        if (_moveDirection != direction)
            return;

        _moveDirection = Vector2Int.zero;
    }
}
}