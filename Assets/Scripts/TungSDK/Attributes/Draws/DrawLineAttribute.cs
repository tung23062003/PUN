using System;

[AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true)]
public class DrawLineAttribute : Attribute
{
    public BaseColor Color;
    public bool IsLocal = false;
    public float Thickness = 1f;

    public DrawLineAttribute(BaseColor color = BaseColor.Red, bool isLocal = false, float thickness = 0)
    {
        this.Color = color;
        this.IsLocal = isLocal;
        this.Thickness = thickness;
    }
}