using System;
using UnityEngine;

/// <summary>
/// Provides an class for accessing protected boolean player preferences via properties, offering a more structured approach than 
/// interacting directly with the static WasdPlayerPrefs class. Also allows to easily assign the protected player preferences
/// in the unity inspector.
/// </summary>
[Serializable]
public class W_Bool : IProtectedPref<bool>
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
    private bool defaultValue;

    [ReadOnly]

    /// <summary>
    /// Gets or sets the value of the player preference.
    /// </summary>
    public bool Value
    {
        get
        {
            return  W_PlayerPrefs.GetBool(key, this.defaultValue);
        }
        set
        {
            W_PlayerPrefs.SetBool(key, value);
        }
    }   
}
