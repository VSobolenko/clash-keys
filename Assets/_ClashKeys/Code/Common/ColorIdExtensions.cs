using ClashKeys.UI;
using UnityEngine;

namespace ClashKeys.Common
{
internal static class ColorIdExtensions
{
    public static Color ToColor(this ColorId colorId)
    {
        int rgb = (int) colorId;
        float r = ((rgb >> 16) & 0xFF) / 255f;
        float g = ((rgb >> 8) & 0xFF) / 255f;
        float b = (rgb & 0xFF) / 255f;

        return new Color(r, g, b, 1f);
    }
}
}