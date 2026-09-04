using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 管理器: 统一管理继承 BasePanel 的面板预制体.
/// 约定: 面板预制体放在 Resources/UI/ 目录下, 预制体名与面板类型名一致.
/// 通过 ShowPanel&lt;T&gt;() / HidePanel&lt;T&gt;() 显示与隐藏, 面板隐藏后缓存复用.
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    /// <summary>面板预制体在 Resources 下的加载路径前缀</summary>
    private const string PanelPathPrefix = "UI/";

    private readonly Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();

    /// <summary>所有面板的根 Canvas</summary>
    public Canvas RootCanvas { get; private set; }

    protected override void OnInit()
    {
        CreateRootCanvas();
        CreateRootEventSystem();
    }

    private void CreateRootEventSystem()
    {
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.transform.SetParent(transform, false);
            eventSystemGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGo.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    /// <summary>获取指定面板, 不存在时返回 null (不会创建)</summary>
    public T GetPanel<T>() where T : BasePanel
    {
        return panels.TryGetValue(typeof(T).Name, out var panel) && panel != null ? (T)panel : null;
    }

    /// <summary>
    /// 显示指定面板: 已存在则直接显示;
    /// 不存在则从 Resources/UI/ 加载同名预制体并实例化.
    /// </summary>
    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panels.TryGetValue(panelName, out var panel) && panel != null)
        {
            panel.Show();
            return (T)panel;
        }

        var prefab = Resources.Load<GameObject>(PanelPathPrefix + panelName);
        GameObject go;
        if (prefab != null)
        {
            go = Instantiate(prefab, RootCanvas.transform, false);
        }
        else
        {
            // 找不到预制体时创建空物体兜底, 便于程序化创建面板
            Debug.LogWarning($"[UIManager] 未找到面板预制体 Resources/{PanelPathPrefix}{panelName}, 已创建空面板");
            go = new GameObject(panelName);
            go.transform.SetParent(RootCanvas.transform, false);
        }

        var newPanel = go.GetComponent<T>();
        if (newPanel == null)
            newPanel = go.AddComponent<T>();
        panels[panelName] = newPanel;
        newPanel.OnInit();
        newPanel.Show();
        return newPanel;
    }

    /// <summary>隐藏指定面板 (缓存保留, 再次 Show 时复用同一实例)</summary>
    public void HidePanel<T>() where T : BasePanel
    {
        if (panels.TryGetValue(typeof(T).Name, out var panel) && panel != null)
            panel.Hide();
    }

    /// <summary>隐藏所有已显示的面板</summary>
    public void HideAllPanels()
    {
        foreach (var panel in panels.Values)
        {
            if (panel != null && panel.IsShow)
                panel.Hide();
        }
    }

    /// <summary>创建根 Canvas (Overlay 模式). 需要接收 UI 点击时, 场景中还需有 EventSystem, 修改比例为 1920x1080</summary>
    private void CreateRootCanvas()
    {
        var canvasGo = new GameObject("RootCanvas");
        canvasGo.transform.SetParent(transform, false);
        RootCanvas = canvasGo.AddComponent<Canvas>();
        RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var RootCanvasScaler = canvasGo.AddComponent<CanvasScaler>();
        RootCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        RootCanvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasGo.AddComponent<GraphicRaycaster>();
    }
}
