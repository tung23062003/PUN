using System;

/// <summary>
/// Vector3, Vector2, Vector3Int, Vector2Int
/// </summary>
public class DrawCubeAttribute : Attribute
{
    public BaseColor Color;
    public float Size = 0.1f;
    public float Alpha = 1;
    public bool IsDisplayName;
    public bool IsLocal = false;
    public DrawCubeAttribute(float size, BaseColor color, bool isLocal, float alpha = 1, bool isDisplayName = false)
    {
        this.IsLocal = isLocal;
        this.Size = size;
        this.Alpha = alpha;
        this.Color = color;
        IsDisplayName = isDisplayName;
    }
}