using System;
using UnityEngine;

namespace ClashKeys.Game.Map
{
internal class Highway : IDisposable
{
    private readonly Settings _settings;
    private readonly Vector3 _center;
    private readonly SpawnRegulator _spawnRegulator;
    private readonly IObstacleCourseFactory _factory;
    private readonly SimpleMoverController _mover;

    public Vector3 WorldDirection => new Vector3(_settings.direction.x, _settings.direction.y, 0).normalized;
    public Vector3 StartPoint => _center - WorldDirection * _settings.length / 2f;
    public Vector3 EndPoint => StartPoint + WorldDirection * _settings.length;

    public Highway(Settings settings, Vector3 center, SpawnRegulator.Settings settingsSpawn,
                   IObstacleCourseFactory factory)
    {
        _settings = settings;
        _center = center;
        _factory = factory;
        _spawnRegulator = new SpawnRegulator(settingsSpawn, false);
        _mover = new SimpleMoverController();

        _spawnRegulator.OnSpawnRequired += ProcessSpawnItem;
    }

    public void Dispose()
    {
        _spawnRegulator.OnSpawnRequired -= ProcessSpawnItem;
    }

    private void ProcessSpawnItem(SpawnRegulator spawner)
    {
        spawner.ConfirmSpawn();
        var item = _factory.Spawn();
        var dir = (StartPoint - EndPoint).normalized;

        item.transform.position = StartPoint;
        item.transform.forward = dir;

        _mover.Add(item.transform, EndPoint, _settings.speed, () => { _factory.DeSpawn(item); });
    }

    public void UpdateView(float deltaTime)
    {
        _mover.Update(deltaTime);
        _spawnRegulator.Update(deltaTime);
    }

    [Serializable]
    internal class Settings
    {
        [SerializeField, Range(1, 100)] public float length = 18;
        [SerializeField] public Vector2 direction = Vector2.left;
        [SerializeField] public float speed = 1f;
    }
}
}