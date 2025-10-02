using System;
using UnityEngine;

/// <summary>
/// Provides an class for accessing protected vector4 player preferences via properties, offering a more structured approach than 
/// interacting directly with the static WasdPlayerPrefs class. Also allows to easily assign the protected player preferences
/// in the unity inspector.
/// </summary>
[Serializable]
public class W_Vector4 : IProtectedPref<Vector4>
{
    /// <summary>
    /// Gets the unique key associated with the player preference.
    /// </summary>
    [SerializeField]
    [Tooltip("The unique key associated with the player preference.")]
    private string key;

    /// <summary>
    /// Gets the unique key associated with the player preference.
    /// </summary>
    public String Key => this.Key;

    /// <summary>
    /// The default value if the player preference is not set.
    /// </summary>
    [SerializeField]
    [Tooltip("The default value if the player preference is not set.")]
    private Vector4 defaultValue;

    /// <summary>
    /// Gets or sets the value of the player preference.
    /// </summary>
    public Vector4 Value
    {
        get
        {
            return W_PlayerPrefs.GetVector4(key, this.defaultValue);
        }
        set
        {
            W_PlayerPrefs.SetVector4(key, value);
        }
    }
}
