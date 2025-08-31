using System;
using System.Collections.Generic;
using ClashKeys.Common;
using ClashKeys.Game.Chest;
using Game.Extensions;
using Game.GUI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ClashKeys.UI
{
public enum ColorId
{
    Green = 0x00FF00,
    Orange = 0xFF8000,
    Red = 0xEF4E4E,
    Blue = 0x0000FF,
    Cyan = 0x00FFFF,
    Yellow = 0xFFFF00,
    Violet = 0x8F00FF,
    Pink = 0xFFC0CB,
}

internal sealed class ChestWindowViewUI : WindowUI
{
    [SerializeField] private TextMeshProUGUI keyCounter;
    [SerializeField] private Image lockView;
    public KeyViewUI[] keys;
    public RectTransform lockArea;

    public void UpdateTextCounter(Lock lockLogic) =>
        keyCounter.text = $"{lockLogic.CurrentKeys}/{lockLogic.RequiredKeys}";

    public void SetLockIdView(ColorId id) => lockView.color = id.ToColor();

    [ContextMenu("Collect keys")]
    private void EditorCollectKeys() => keys = GetComponentsInChildren<KeyViewUI>();
}

internal sealed class ChestWindowMediatorUI : BaseMediator<ChestWindowViewUI>
{
    private Lock _lock;

    public ChestWindowMediatorUI(ChestWindowViewUI window) : base(window)
    {
    }

    public override void OnInitialize()
    {
        if (window.keys.Length == 0)
            throw new ArgumentNullException();

        const int requiredKeys = 3;
        var lockLogic = CreateLock(requiredKeys);
        var keysIds = GetChestKeysId(requiredKeys, window.keys.Length, lockLogic.TargetColor);

        for (var i = 0; i < window.keys.Length; i++)
        {
            var keyView = window.keys[i];
            var keyId = keysIds[i];

            keyView.OnFinishMove += TryOpenLock;
            keyView.SetId(keyId);
        }
    }

    public override void OnDestroy()
    {
        foreach (var keyView in window.keys)
        {
            if (keyView == null)
                return;

            keyView.OnFinishMove -= TryOpenLock;
        }
    }

    private Lock CreateLock(int requiredKeys)
    {
        var lockId = default(ColorId).Random();
        _lock = new Lock(lockId, requiredKeys);

        window.SetLockIdView(lockId);
        window.UpdateTextCounter(_lock);

        return _lock;
    }

    private static IList<ColorId> GetChestKeysId(int activatedCount, int totalCount, ColorId targetColor)
    {
        if (activatedCount > totalCount)
            throw new ArgumentOutOfRangeException();

        var keys = new List<ColorId>(totalCount);

        for (var i = 0; i < totalCount; i++)
            keys.Add(i < activatedCount ? targetColor : default(ColorId).Random(targetColor));

        return keys.Shuffle();
    }

    private void TryOpenLock(KeyViewUI key)
    {
        if (window.lockArea.IsOverlapping(key.rectTransform) == false)
        {
            key.SetInitialPosition();

            return;
        }

        if (_lock.TryUnlock(key.Id) == false)
        {
            key.SetInitialPosition();

            return;
        }

        Object.Destroy(key.gameObject);
        window.UpdateTextCounter(_lock);

        if (_lock.IsUnlocked)
            OnLockOpened?.Invoke();
    }

    public event Action OnLockOpened;
}
}