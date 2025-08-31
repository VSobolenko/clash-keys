using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace ClashKeys.Game.Fighting
{
internal class BulletController : ITickable
{
    private readonly List<BulletData> _bullets = new(16);
    private readonly float _defaultSpeed = 2f;

    public void Add(IBulletSource bullet, Vector3 targetPos, Action releaseAction, float speed = -1f)
    {
        if (bullet == null || bullet.transform == null)
            return;

        _bullets.Add(new BulletData
        {
            Bullet = bullet,
            TargetPos = targetPos,
            ReleaseAction = releaseAction,
            Speed = speed > 0 ? speed : _defaultSpeed
        });
    }

    public void Remove(IBulletSource bullet)
    {
        for (var i = _bullets.Count - 1; i >= 0; i--)
        {
            if (_bullets[i].Bullet == bullet)
            {
                _bullets[i].ReleaseAction?.Invoke();
                _bullets.RemoveAt(i);
            }
        }
    }

    public void Clear()
    {
        for (var i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].ReleaseAction?.Invoke();
        }

        _bullets.Clear();
    }

    public void Tick()
    {
        for (var i = _bullets.Count - 1; i >= 0; i--)
        {
            var b = _bullets[i];
            if (b.Bullet == null || b.Bullet.transform == null)
            {
                _bullets.RemoveAt(i);

                continue;
            }

            var t = b.Bullet.transform;

            t.position = Vector3.MoveTowards(
                t.position,
                b.TargetPos,
                b.Speed * Time.deltaTime
            );

            if (Vector3.Distance(t.position, b.TargetPos) < 0.01f)
            {
                b.ReleaseAction?.Invoke();
                _bullets.RemoveAt(i);
            }
        }
    }

    private class BulletData
    {
        public IBulletSource Bullet;
        public Vector3 TargetPos;
        public Action ReleaseAction;
        public float Speed;
    }
}
}