using UnityEngine;

namespace ClashKeys.Common
{
internal static class RectTransformExtensions
{
    // overlapPercent = [0,1]
    public static bool IsOverlapping(this RectTransform a, RectTransform b)
    {
        var cornersA = new Vector3[4];
        var cornersB = new Vector3[4];
        a.GetWorldCorners(cornersA);
        b.GetWorldCorners(cornersB);

        var rectA = new Rect(cornersA[0].x, cornersA[0].y, cornersA[2].x - cornersA[0].x, cornersA[2].y - cornersA[0].y);
        var rectB = new Rect(cornersB[0].x, cornersB[0].y, cornersB[2].x - cornersB[0].x, cornersB[2].y - cornersB[0].y);

        return rectA.Overlaps(rectB);
    }
}
}