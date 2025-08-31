using Game.Pools;
using UnityEngine;

namespace ClashKeys.Game.Fighting
{
internal class SimpleWeapon<TBullet> where TBullet : BasePooledObject, IBulletSource
{
    private readonly Transform _firePoint;
    private readonly TBullet _bulletPrefab;
    private readonly IObjectPoolManager _objectPoolManager;
    private readonly BulletController _bulletController;

    private readonly float _cooldown;
    private float _timeSinceLastShot;

    public SimpleWeapon(Transform firePoint, TBullet bulletPrefab, float cooldown, IObjectPoolManager objectPoolManager,
                        BulletController bulletController)
    {
        _firePoint = firePoint;
        _bulletPrefab = bulletPrefab;
        _cooldown = cooldown;
        _objectPoolManager = objectPoolManager;
        _bulletController = bulletController;
        _timeSinceLastShot = Random.Range(0, cooldown);

        objectPoolManager.Prepare(bulletPrefab, 4);
    }

    public void Update(float deltaTime)
    {
        if (_timeSinceLastShot < _cooldown)
            _timeSinceLastShot += deltaTime;
    }

    public bool TryShoot(Vector3 targetPosition, out TBullet bullet)
    {
        bullet = null;

        if (_timeSinceLastShot < _cooldown)
            return false;

        _timeSinceLastShot = 0f;

        var position = _firePoint.position;
        var instance = _objectPoolManager.Get<TBullet>(_bulletPrefab, position, Quaternion.identity);

        _bulletController.Add(instance, targetPosition, () =>
        {
            if (instance == null)
                return;

            _objectPoolManager.Release(instance);
        });

        var dir = (targetPosition - position).normalized;
        instance.transform.forward = dir;

        bullet = instance;

        return true;
    }
}
}