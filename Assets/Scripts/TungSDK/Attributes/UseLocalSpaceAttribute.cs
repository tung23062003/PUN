using System;

public class UseLocalSpaceAttribute : SettingsAttribute
{
    public string transformField;

    public UseLocalSpaceAttribute()
    {
    }

    public UseLocalSpaceAttribute(string parent)
    {
        this.transformField = parent;
    }
}