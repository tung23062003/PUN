using System;

public class DrawSphereAttribute : Attribute
{
    public BaseColor Color;
    public float Size;
    public float Alpha = 1;
    public bool IsLocal = false;
    public DrawSphereAttribute(float size, BaseColor color, bool isLocal = false, float alpha = 1)
    {
        this.Size = size;
        this.Color = color;
        this.IsLocal = isLocal;
        Alpha = alpha;
    }
}