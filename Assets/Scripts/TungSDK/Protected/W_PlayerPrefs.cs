using System;
using UnityEngine;

/// <summary>
/// Protected version of the unity PlayerPrefs. Contains also additional save and load able types.
/// </summary>
public sealed class W_PlayerPrefs
{
    /// <summary>
    /// Returns true if key exists in the preferences.
    /// </summary>
    /// <param name="_key"></param>
    /// <returns></returns>
    public static bool HasKey(string _key)
    {
        return PlayerPrefs.HasKey(GetKeyProtected(_key));
    }

    /// <summary>
    ///   <para>Sets the _value of the preference identified by _key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetInt(string _key, int _value)
    {
        // Init Protected with value _value.
        ProtectedInt32 var_Protected = new ProtectedInt32(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out Int32 var_ObfuscatedValue, out Int32 var_Secret);
            // Set intern value as value for _key
            PlayerPrefs.SetInt(GetKeyProtected(_key), var_ObfuscatedValue);
            // Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), var_Secret);
        }
        else
        {
            PlayerPrefs.SetInt(GetKeyProtected(_key), _value);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static int GetInt(string _key, int _defaultValue = 0)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedInt32 var_Protected = new ProtectedInt32(0);
            if(IsProtected())
            {
                // Load obuscated value.
                int var_ObfuscatedValue = PlayerPrefs.GetInt(GetKeyProtected(_key));

                // Load secret.
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize(var_ObfuscatedValue, var_Secret);
            }
            else
            {
                var_Protected.Value = PlayerPrefs.GetInt(GetKeyProtected(_key));
            }
            return var_Protected.Value;
        }

        return PlayerPrefs.GetInt(_key, _defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static int GetInt(string _key)
    {
        return GetInt(_key, 0);
    }

    /// <summary>
    ///   <para>Sets the _value of the preference identified by _key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetFloat(string _key, float _value)
    {
        if(IsProtected())
        {
            // Init Protected with value _value.
            ProtectedFloat var_Protected = new ProtectedFloat(_value);
            // Serialize the protected.
            var_Protected.Serialize(out UInt32 var_ObfuscatedValue, out UInt32 var_Secret);
            //Set intern value as value for _key
            PlayerPrefs.SetInt(GetKeyProtected(_key), (int)var_ObfuscatedValue);
            //Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), (int)var_Secret);
        }
        else
        {
            PlayerPrefs.SetFloat(GetKeyProtected(_key), _value);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static float GetFloat(string _key, float _defaultValue = 0)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedFloat var_Protected = new ProtectedFloat(0);
            if(IsProtected())
            {
                // Load obfuscated value.
                float var_ObfuscatedValue = PlayerPrefs.GetFloat(GetKeyProtected(_key));

                // Load secret
                float var_Secret = PlayerPrefs.GetFloat(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize((UInt32)var_ObfuscatedValue, (UInt32)var_Secret);
            }
            else
            {
                var_Protected.Value = PlayerPrefs.GetFloat(GetKeyProtected(_key));
            }
            return var_Protected.Value;
        }

        return PlayerPrefs.GetFloat(_key, _defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static float GetFloat(string _key)
    {
        return GetFloat(_key, 0);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetString(string _key, string _value)
    {
        //Init Protected with value _value
        ProtectedString var_Protected = new ProtectedString(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out string var_ObfuscatedValue, out Int32 var_Secret);
            //Set intern value as value for _key
            PlayerPrefs.SetString(GetKeyProtected(_key), var_ObfuscatedValue);
            //Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), var_Secret);
        }
        else
        {
            PlayerPrefs.SetString(GetKeyProtected(_key), _value);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static string GetString(string _key, string _defaultValue = "")
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedString var_Protected = new ProtectedString(String.Empty);

            if(IsProtected())
            {
                // Load intern Value
                string var_ObfuscatedValue = PlayerPrefs.GetString(GetKeyProtected(_key));

                // Load Name
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize(var_ObfuscatedValue, var_Secret);
            }
            else
            {
                string _Value = PlayerPrefs.GetString(GetKeyProtected(_key));
                var_Protected.Value = _Value;
            }
            return var_Protected.Value;
        }

        return PlayerPrefs.GetString(_key, _defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static string GetString(string _key)
    {
        return GetString(_key, "");
    }

    ////////////////////// CUSTOM ////////////////////////////

    /// <summary>
    ///   <para>Sets the _value of the preference identified by _key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetBool(string _key, bool _value)
    {
        // Init Protected with value _value.
        ProtectedBool var_Protected = new ProtectedBool(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out byte var_ObfuscatedValue, out int var_Secret);
            // Set intern value as value for _key.
            PlayerPrefs.SetString(GetKeyProtected(_key), var_ObfuscatedValue.ToString());
            // Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), var_Secret);
        }
        else
        {
            PlayerPrefs.SetString(GetKeyProtected(_key), _value.ToString());
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static bool GetBool(string _key, bool _defaultValue = false)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedBool var_Protected = new ProtectedBool(false);
            if(IsProtected())
            {
                // Load obfuscated value.
                string var_ObfuscatedValueString = PlayerPrefs.GetString(GetKeyProtected(_key));
                // Load Secret
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));
                // Deserialize the protected.
                var_Protected.Deserialize(byte.Parse(var_ObfuscatedValueString), var_Secret);
            }
            else
            {
                string _Value = PlayerPrefs.GetString(GetKeyProtected(_key));
                var_Protected.Value = bool.Parse(_Value);
            }
            // Return the unobfuscated value.
            return var_Protected.Value;
        }

        return _defaultValue;
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static bool GetBool(string _key)
    {
        return GetBool(_key, false);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetVector2(string _key, Vector2 _value)
    {
        //Init Protected with value _value
        ProtectedVector2 var_Protected = new ProtectedVector2(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out UInt32 var_ObfuscatedValueX, out UInt32 var_ObfuscatedValueY, out UInt32 var_Secret);
            //Set intern value as value for _key
            PlayerPrefs.SetString(GetKeyProtected(_key), var_ObfuscatedValueX + "|" + var_ObfuscatedValueY);
            //Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), (int)var_Secret);
        }
        else
        {
            PlayerPrefs.SetString(GetKeyProtected(_key), _value.x + "|" + _value.y);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static Vector2 GetVector2(string _key, Vector2 _defaultValue)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedVector2 var_Protected = new ProtectedVector2(Vector2.zero);
            if(IsProtected())
            {
                // Load intern Value
                string var_ObfuscatedValueString = PlayerPrefs.GetString(GetKeyProtected(_key));
                string[] var_ObfuscatedValueStringSplit = var_ObfuscatedValueString.Split('|');

                // Load Name
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize(UInt32.Parse(var_ObfuscatedValueStringSplit[0]), UInt32.Parse(var_ObfuscatedValueStringSplit[1]), (UInt32)var_Secret);
            }
            else
            {
                string _Value = PlayerPrefs.GetString(GetKeyProtected(_key));
                var_Protected.Value = new Vector2(float.Parse(_Value.Split('|')[0]), float.Parse(_Value.Split('|')[1]));
            }

            return var_Protected.Value;
        }

        return _defaultValue;
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static Vector2 GetVector2(string _key)
    {
        return GetVector2(_key, Vector2.zero);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetVector3(string _key, Vector3 _value)
    {
        //Init Protected with value _value
        ProtectedVector3 var_Protected = new ProtectedVector3(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out UInt32 var_ObfuscatedValueX, out UInt32 var_ObfuscatedValueY, out UInt32 var_ObfuscatedValueZ, out UInt32 var_Secret);
            //Set intern value as value for _key
            PlayerPrefs.SetString(GetKeyProtected(_key), var_ObfuscatedValueX + "|" + var_ObfuscatedValueY + "|" + var_ObfuscatedValueZ);
            //Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), (int)var_Secret);
        }
        else
        {
            PlayerPrefs.SetString(GetKeyProtected(_key), _value.x + "|" + _value.y + "|" + _value.z);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static Vector3 GetVector3(string _key, Vector3 _defaultValue)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedVector3 var_Protected = new ProtectedVector3(Vector3.zero);
            if(IsProtected())
            {

                // Load obfuscated value.
                string var_ObfuscatedValueString = PlayerPrefs.GetString(GetKeyProtected(_key));
                string[] var_ObfuscatedValueStringSplit = var_ObfuscatedValueString.Split('|');

                // Load secret.
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize(UInt32.Parse(var_ObfuscatedValueStringSplit[0]), UInt32.Parse(var_ObfuscatedValueStringSplit[1]), UInt32.Parse(var_ObfuscatedValueStringSplit[2]), (UInt32)var_Secret);
            }
            else
            {
                string _Value = PlayerPrefs.GetString(GetKeyProtected(_key));
                var_Protected.Value = new Vector3(float.Parse(_Value.Split('|')[0]), float.Parse(_Value.Split('|')[1]), float.Parse(_Value.Split('|')[2]));
            }
            return var_Protected.Value;
        }

        return _defaultValue;
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static Vector3 GetVector3(string _key)
    {
        return GetVector3(_key, Vector3.zero);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetVector4(string _key, Vector4 _value)
    {
        // Init Protected with value _value
        ProtectedVector4 var_Protected = new ProtectedVector4(_value);
        if(IsProtected())
        {
            // Serialize the protected.
            var_Protected.Serialize(out UInt32 var_ObfuscatedValueX, out UInt32 var_ObfuscatedValueY, out UInt32 var_ObfuscatedValueZ, out UInt32 var_ObfuscatedValueW, out UInt32 var_Secret);
            // Set intern value as value for _key
            PlayerPrefs.SetString(GetKeyProtected(_key), var_ObfuscatedValueX + "|" + var_ObfuscatedValueY + "|" + var_ObfuscatedValueZ + "|" + var_ObfuscatedValueW);

            // Save under the _key+_ProtectedHash, the secret.
            PlayerPrefs.SetInt(GetKeyProtectedHash(_key), (int)var_Secret);
        }
        else
        {
            PlayerPrefs.SetString(GetKeyProtected(_key), _value.x + "|" + _value.y + "|" + _value.z + "|" + _value.w);
        }
        // Auto save if activated.
        if(AutoSave)
        {
            Save();
        }
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_defaultValue"></param>
    public static Vector4 GetVector4(string _key, Vector4 _defaultValue)
    {
        if(PlayerPrefs.HasKey(GetKeyProtected(_key)))
        {
            // Create empty protected by not using the empty constructor. The empty constructor would initialize the struct with the default / empty values.
            ProtectedVector4 var_Protected = new ProtectedVector4(Vector4.zero);
            if(IsProtected())
            {
                // Load obfuscated value.
                string var_ObfuscatedValueString = PlayerPrefs.GetString(GetKeyProtected(_key));
                string[] var_ObfuscatedValueStringSplit = var_ObfuscatedValueString.Split('|');

                // Load secret.
                int var_Secret = PlayerPrefs.GetInt(GetKeyProtectedHash(_key));

                // Deserialize the protected.
                var_Protected.Deserialize(UInt32.Parse(var_ObfuscatedValueStringSplit[0]), UInt32.Parse(var_ObfuscatedValueStringSplit[1]), UInt32.Parse(var_ObfuscatedValueStringSplit[2]), UInt32.Parse(var_ObfuscatedValueStringSplit[3]), (UInt32)var_Secret);
            }
            else
            {
                string _Value = PlayerPrefs.GetString(GetKeyProtected(_key));
                var_Protected.Value = new Vector4(float.Parse(_Value.Split('|')[0]), float.Parse(_Value.Split('|')[1]), float.Parse(_Value.Split('|')[2]), float.Parse(_Value.Split('|')[3]));
            }
            return var_Protected.Value;
        }

        return _defaultValue;
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static Vector4 GetVector4(string _key)
    {
        return GetVector4(_key, Vector4.zero);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_value"></param>
    public static void SetQuaternion(string _key, Quaternion _value)
    {
        Vector4 var_Vector = new Vector4(_value.x, _value.y, _value.z, _value.w);
        SetVector4(_key, var_Vector);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static Quaternion GetQuaternion(string _key, Quaternion _default)
    {
        Vector4 var_Vector = GetVector4(_key, new Vector4(_default.x, _default.y, _default.z, _default.w));
        return new Quaternion(var_Vector.x, var_Vector.y, var_Vector.z, var_Vector.w);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to _key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="_key"></param>
    public static Quaternion GetQuaternion(string _key)
    {
        return GetQuaternion(_key, Quaternion.identity);
    }

    /// <summary>
    /// Activate or deactivate force autosaving of modified preferences to the disk.
    /// </summary>
    public static bool AutoSave = false;

    /// <summary>
    /// Writes all modified preferences to disk.
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Removes key and its corresponding value from the preferences.
    /// </summary>
    public static void DeleteKey(string _key)
    {
        PlayerPrefs.DeleteKey(GetKeyProtected(_key));

        PlayerPrefs.DeleteKey(GetKeyProtectedHash(_key));
    }
    static string GetKeyProtected(string _key)
    {
        return _key + (IsProtected() ? "_Protected" : "");
    }
    static string GetKeyProtectedHash(string _key)
    {
        return _key + (IsProtected() ? "_ProtectedHash" : "");
    }
    static bool IsProtected()
    {
#if UNITY_EDITOR
        return false;
#else
        return true;
#endif
    }
}
