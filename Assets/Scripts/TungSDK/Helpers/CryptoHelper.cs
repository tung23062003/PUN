using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class CryptoHelper 
{
    private static string key = "wasdmobile.com-key-secret-123456"; // 32 ký tự (AES-256)
    private static string iv = "-wasdmobile.com-"; // 16 ký tự (AES IV)

    /// <summary>
    /// Mã hóa một chuỗi JSON bằng AES
    /// </summary>
    public static string Encrypt(this string json)
    {
        try
        {
            using(Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] inputBytes = Encoding.UTF8.GetBytes(json);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Lỗi mã hóa: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Giải mã một chuỗi JSON bằng AES
    /// </summary>
    public static string Decrypt(this string encryptedJson)
    {
        try
        {
            using(Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedJson);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Lỗi giải mã: {e.Message}");
            return null;
        }
    }
}
