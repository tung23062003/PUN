/// <summary>
/// It locks the value of a specific axis.
/// </summary>
public class UseAxisLockAttribute : SettingsAttribute
{
    public Axis axis;

    public UseAxisLockAttribute(Axis axis)
    {
        this.axis = axis;
    }
}

public enum Axis { X, Y, Z }
