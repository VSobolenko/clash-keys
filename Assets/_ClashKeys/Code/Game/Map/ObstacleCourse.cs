using UnityEngine;

namespace ClashKeys.Game.Map
{
internal class ObstacleCourse : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private float _length;
    [SerializeField] private Vector2 _direction;

    public Vector3 WorldDirection => new Vector3(_direction.x, _direction.y, 0).normalized;
    public Vector3 StartPoint => transform.position - WorldDirection * _length / 2f;
    public Vector3 EndPoint => StartPoint + WorldDirection * _length;

    [SerializeField] private SpawnRegulator.Settings _settings;

    private SpawnRegulator _spawnRegulator;

    private void Start()
    {
        _spawnRegulator = new SpawnRegulator(_settings, false);
        _spawnRegulator.OnSpawnRequired += regulator => { regulator.ConfirmSpawn(); };
    }

    private void Update()
    {
        _spawnRegulator.Update(Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(StartPoint, EndPoint);

        var arrowHead = EndPoint;
        var right = Quaternion.Euler(0, 0, 150) * WorldDirection * 0.5f;
        var left = Quaternion.Euler(0, 0, -150) * WorldDirection * 0.5f;

        Gizmos.DrawLine(arrowHead, arrowHead + right);
        Gizmos.DrawLine(arrowHead, arrowHead + left);
    }
}
}