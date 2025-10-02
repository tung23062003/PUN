using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu]
public class PanelConfig : ScriptableObject
{
    public List<BaseUI> Panels;
#if UNITY_EDITOR
    public static PanelConfig Get()
    {
        var config = Resources.Load<PanelConfig>("PanelConfig");
        if (config != null) return config;
        config = CreateInstance<PanelConfig>();
        AssetDatabase.CreateAsset(config, "Assets/Resources/PanelConfig.asset");
        config.Panels = new List<BaseUI>();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return config;
    }
#endif
}
