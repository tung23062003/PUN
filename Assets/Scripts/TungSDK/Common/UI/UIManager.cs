using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    static GameObject UIRoot { get; set; }
    static PanelConfig Config { get; set; }
    static Dictionary<string, BaseUI> Panels;
    public static BaseUI PrevUI { get; private set; }
    public static Canvas Canvas { get; private set; }
    static CanvasScaler canvasScaler { get; set; }
    static UIManager Instance { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnInit()
    {
        if(IsValidated())
        {
            InitUI();
        }
    }

    static bool IsValidated()
    {
        return WasdExtensions.IsInBuildSetting();
    }

    static void InitUI()
    {
        Config = Resources.Load<PanelConfig>(nameof(PanelConfig));
        SpawnRoot();
        InitPanels();
        Object.DontDestroyOnLoad(UIRoot);
    }
    public static Canvas CreateCanvas(string name, int sortingOrder)
    {
        return SpawnCanvas(name, sortingOrder);
    }

    static void InitPanels()
    {
        if(Panels == null)
        {
            Panels = new Dictionary<string, BaseUI>();
        }
        if(Config == null) return;
        foreach(var _panel in Config.Panels)
        {
            var panel = SpawnUI(_panel, UIRoot);
            Panels.Add(_panel.GetType().Name, panel);
        }
        Wasd.LogStateInitialize("Panels");
    }
    /// <summary>
    /// Spawns the root UI GameObject and sets up its components.
    /// </summary>
    static void SpawnRoot()
    {
        if(UIRoot != null) return;
        Canvas = SpawnCanvas("UI ROOT", 10);
        UIRoot = Canvas.gameObject;
        UIRoot.AddComponent<GraphicRaycaster>();
        Object.DontDestroyOnLoad(UIRoot);
    }

    public static void AddPanels(List<BaseUI> panels, GameObject root)
    {
        if(Panels == null)
        {
            Panels = new Dictionary<string, BaseUI>();
        }
        foreach(var _panel in panels)
        {
            var panel = SpawnUI(_panel, root, false);
            Panels.Add(_panel.GetType().Name, panel);
        }
    }
    public static void SetResolution(int width, int height)
    {
        canvasScaler.referenceResolution = new Vector2(width, height);
    }
    static Canvas SpawnCanvas(string name, int sortingOrder)
    {
        UIRoot = new GameObject(name);
        var canvas = UIRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;
        canvasScaler = UIRoot.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
        }
        else
        {
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
        }

        canvasScaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }
    public static void HideAllPanel()
    {
        if(Panels == null)
        {
            Panels = new Dictionary<string, BaseUI>();
        }
        foreach(var panel in Panels)
        {
            PrevUI = null;
            panel.Value.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Hides the specified panel.
    /// </summary>
    /// <typeparam name="T">The type of the panel to hide. This type must derive from BaseUI.</typeparam>
    public static void HidePanel<T>() where T : BaseUI
    {
        string name = (typeof(T).Name);
        if(Panels == null)
        {
            Panels = new Dictionary<string, BaseUI>();
        }
        if(Panels.TryGetValue(name, out var panel))
        {
            PrevUI = panel;
            panel.gameObject.SetActive(false);
            if(panel.transform.parent == null)
            {
                panel.transform.SetParent(UIRoot.transform, false);
            }
        }
    }

    public static T ShowPanel<T>(float duration = 1) where T : BaseUI
    {
        if(UIRoot == null)
        {
            SpawnRoot();
        }
        string name = (typeof(T).Name);

        if(Panels == null)
        {
            Panels = new Dictionary<string, BaseUI>();
        }
        if(Panels.TryGetValue(name, out var panel))
        {
            panel.gameObject.SetActive(true);
        }
        else
        {
            panel = SpawnUI<T>();
            Panels.Add(name, panel);
            panel.gameObject.SetActive(true);
        }
        if(panel.transform.parent == null)
        {
            panel.transform.SetParent(UIRoot.transform, false);
        }
        panel.SetInfo();
        panel.Show(duration);
        panel.transform.SetAsLastSibling();
        return panel as T;
    }

    public static T Panel<T>() where T : BaseUI
    {
        string name = typeof(T).Name;
        foreach(var p in Panels)
        {
            if(p.Key == name) return p.Value as T;
        }
        return null;
    }

    protected static BaseUI SpawnUI(BaseUI _panel, GameObject _root, bool _createNew = true)
    {
        if(_root == null) _root = UIRoot;

        var panel = _panel;
        if(_createNew)
            panel = Object.Instantiate(_panel);
        panel.transform.SetParent(_root.transform, false);
        panel.gameObject.SetActive(false);
        panel.name = _panel.name;
        return panel;
    }
    protected static T SpawnUI<T>(bool isActivate = false) where T : BaseUI
    {
        string name = (typeof(T).Name);
        //T rs = Resources.Load<T>($"UI/{name}");
        T rs = GetPanel<T>();
        Debug.Assert(rs != null);
        var panel = Object.Instantiate<T>(rs);

        panel.transform.SetParent(UIRoot.transform, false);

        panel.gameObject.SetActive(isActivate);
        panel.name = name;
        panel.hideFlags = HideFlags.None;
        //panel.Init();
        return panel;
    }
    static T GetPanel<T>() where T : BaseUI
    {
        foreach(var p in Config.Panels)
        {
            if(p is T) return p as T;
        }
        return default;
    }
}
