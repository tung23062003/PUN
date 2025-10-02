using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
[Conditional("UNITY_EDITOR")]
public class ProgressAttribute : PropertyAttribute
{
    public float Min, Max;
    public string MethodName;
    public string SliderName;
    public bool UseSlider = false;
    public ProgressAttribute(float min, float max, string methodName)
    {
        UseSlider = false;
        Min = min;
        Max = max;
        MethodName = methodName;
    }
    public ProgressAttribute(string sliderName, string methodName)
    {
        UseSlider = true;
        this.SliderName = sliderName;
        MethodName = methodName;
    }
}
