using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClashKeys.Game.Map
{
internal class SimpleMoverController
{
    private class MovingObject
    {
        public Transform Transform;
        public Vector3 Target;
        public float Speed;
        public Action OnReached;
        public bool IsStopped;
    }

    private readonly List<MovingObject> _objects = new(64);

    public void Add(Transform transform, Vector3 target, float speed, Action onReached)
    {
        _objects.Add(new MovingObject
        {
            Transform = transform,
            Target = target,
            Speed = speed,
            OnReached = onReached,
            IsStopped = false
        });
    }

    public void RemoveObject(Transform transform)
    {
        for (var i = 0; i < _objects.Count; i++)
        {
            if (_objects[i].Transform != transform)
                continue;

            _objects.RemoveAt(i);

            break;
        }
    }

    public void Update(float deltaTime)
    {
        for (var i = _objects.Count - 1; i >= 0; i--)
        {
            var obj = _objects[i];

            if (obj.IsStopped) continue;

            var dir = obj.Target - obj.Transform.position;
            var dist = dir.magnitude;

            if (dist <= obj.Speed * deltaTime)
            {
                obj.Transform.position = obj.Target;
                obj.OnReached?.Invoke();
                _objects.RemoveAt(i);
            }
            else
            {
                obj.Transform.position += dir.normalized * obj.Speed * deltaTime;
            }
        }
    }

    public void StopObject(Transform transform)
    {
        foreach (var obj in _objects)
        {
            if (obj.Transform != transform)
                continue;

            obj.IsStopped = true;

            break;
        }
    }

    public void ResumeObject(Transform transform)
    {
        foreach (var obj in _objects)
        {
            if (obj.Transform != transform)
                continue;

            obj.IsStopped = false;

            break;
        }
    }
}
}