using ClashKeys.UI;

namespace ClashKeys.Game.Chest
{
internal class Lock
{
    public ColorId TargetColor { get; }
    public int RequiredKeys { get; }
    public int CurrentKeys { get; private set; }
    public bool IsUnlocked => CurrentKeys >= RequiredKeys;

    public Lock(ColorId targetColor, int requiredKeys)
    {
        if (requiredKeys <= 0)
            throw new System.ArgumentException("Non zero keys.", nameof(requiredKeys));

        TargetColor = targetColor;
        RequiredKeys = requiredKeys;
        CurrentKeys = 0;
    }

    public bool TryUnlock(ColorId keyId)
    {
        if (IsUnlocked) return false;

        if (keyId != TargetColor)
            return false;

        CurrentKeys++;

        return true;
    }
}
}