using UnityEngine;

public static class TransformExtensions
{
    public static RectTransform GetRectTransform(this Transform t)
    {
        return t as RectTransform;
    }
}

