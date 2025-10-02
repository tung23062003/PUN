using System;
using System.ComponentModel;
using UnityEngine;

[AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true)]
public class DrawCircleAttribute : Attribute
{
    public BaseColor Color;
    public bool IsDisplayName;
    public float Radius = 0.1f;   
    public QuationCircle Rotation;
    public DrawCircleAttribute(BaseColor color, QuationCircle rotation = QuationCircle.XY, bool isDisplayName = false)
    {
        Color = color;
        Rotation = rotation;
        IsDisplayName = isDisplayName;
    }
}