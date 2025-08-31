using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ClashKeys.Game.Map
{
internal class SpawnRegulator
{
    private readonly Settings _settings;

    public event Action<SpawnRegulator> OnSpawnRequired;

    private float _frequencyPerSecond;
    private float _accumulator;

    public SpawnRegulator(Settings settings, bool delayFirstSpawn)
    {
        _settings = settings;
        _accumulator = delayFirstSpawn ? 0f : 1f;
        UpdateFrequencyPerSecond();
    }

    public void Update(float deltaTime)
    {
        _accumulator += deltaTime * _frequencyPerSecond;

        if (_accumulator > 1f)
            ProcessNotifySpawn();
    }

    private void ProcessNotifySpawn() => OnSpawnRequired?.Invoke(this);

    public void ConfirmSpawn()
    {
        _accumulator = 0f;
        UpdateFrequencyPerSecond();
    }

    private void UpdateFrequencyPerSecond()
    {
        switch (_settings.mode)
        {
            case SpawnMode.ConstantFrequency:
                _frequencyPerSecond = _settings.frequencyPerSecond;

                break;
            case SpawnMode.RandomFrequency:
                var min = _settings.randomFrequencyPerSecond.x;
                var max = _settings.randomFrequencyPerSecond.y;
                _frequencyPerSecond = Random.Range(min, max);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Serializable]
    internal class Settings
    {
        private const float MinValue = 0.1f, MaxValue = 10f;

        public SpawnMode mode = SpawnMode.ConstantFrequency;

        [ShowIf(nameof(mode), SpawnMode.ConstantFrequency), MinValue(MinValue), MaxValue(MaxValue), AllowNesting,]
        public float frequencyPerSecond = 1;

        [ShowIf(nameof(mode), SpawnMode.RandomFrequency), MinMaxSlider(MinValue, MaxValue), AllowNesting,]
        public Vector2 randomFrequencyPerSecond = new(0.5f, 1.5f);
    }

    internal enum SpawnMode : byte
    {
        ConstantFrequency = 0,
        RandomFrequency = 1,
    }
}
}