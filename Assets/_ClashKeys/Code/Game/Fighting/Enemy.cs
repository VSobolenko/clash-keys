using Game.AssetContent;
using Game.Pools;
using UnityEngine;
using VContainer;

namespace ClashKeys.Game.Fighting
{
internal class Enemy
{
    private readonly IResourceFactoryManager _resourceFactory;
    private readonly IResourceManager _resourceManager;
    private readonly IObjectPoolManager _objectPoolManager;
    private readonly BulletController _bulletController;

    public EnemyView View { get; set; }
    public int Hp { get; set; }

    public Enemy(IObjectResolver resolver, Transform parent)
    {
        Hp = 1;
        _resourceManager = resolver.Resolve<IResourceManager>();
        _resourceFactory = resolver.Resolve<IResourceFactoryManager>();
        _objectPoolManager = resolver.Resolve<IObjectPoolManager>();
        _bulletController = resolver.Resolve<BulletController>();
        CreateView(parent);
        AttachWeapon();
    }

    private void CreateView(Transform parent)
    {
        View = _resourceFactory.CreateGameObjectWithComponent<EnemyView>("Enemy", parent.position, Quaternion.identity, parent);
    }

    public void AttachWeapon()
    {
        var bulletPrefab = _resourceManager
                           .LoadAsset<GameObject>("Bullet")
                           .GetComponent<BulletView>();

        var weapon = new SimpleWeapon<BulletView>(View.transform, bulletPrefab, 3f, _objectPoolManager, _bulletController);
        View.AttackWeapon(weapon);
    }

    public void AttackPlayer(Vector3 playerPos)
    {
        if (View.Weapon.TryShoot(playerPos, out var bullet) == false)
            return;

        bullet.Source = View.transform;
    }

    public void DisposeWeapon()
    {
        View.SkipWeapon();
    }
}
}