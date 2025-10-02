using System;
using UnityEngine;

/// <summary>
/// Provides an class for accessing protected float player preferences via properties, offering a more structured approach than 
/// interacting directly with the static WasdPlayerPrefs class. Also allows to easily assign the protected player preferences
/// in the unity inspector.
/// </summary>
[Serializable]
public class W_Float : IProtectedPref<Single>
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
    private Single defaultValue;

    /// <summary>
    /// Gets or sets the value of the player preference.
    /// </summary>
    public Single Value
    {
        get
        {
            return W_PlayerPrefs.GetFloat(key, this.defaultValue);
        }
        set
        {
            W_PlayerPrefs.SetFloat(key, value);
        }
    }
}