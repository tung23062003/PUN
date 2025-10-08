using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string GetSavePath(string fileName)
    {
        // Đảm bảo file có đuôi .json
        if (!fileName.EndsWith(".json")) fileName += ".json";
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    /// <summary>
    /// Lưu dữ liệu bất kỳ (object, list, dictionary, v.v.) vào file.
    /// </summary>
    public static void Save<T>(T data, string fileName)
    {
        try
        {
            var path = GetSavePath(fileName);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log($"Saved data ({typeof(T).Name}) to: {path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save {fileName}: {ex}");
        }
    }

    /// <summary>
    /// Tải dữ liệu từ file, trả về mặc định nếu không có hoặc lỗi.
    /// </summary>
    public static T Load<T>(string fileName)
    {
        try
        {
            var path = GetSavePath(fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"File {fileName} not found, returning default.");
                return default;
            }

            var json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<T>(json);
            Debug.Log($"Loaded data ({typeof(T).Name}) from: {path}");
            return data;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load {fileName}: {ex}");
            return default;
        }
    }

    /// <summary>
    /// Xóa file dữ liệu nếu có.
    /// </summary>
    public static void Delete(string fileName)
    {
        var path = GetSavePath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted save file: {path}");
        }
    }
}
