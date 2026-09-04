using UnityEngine;

/// <summary>
/// UI 面板基类. 面板预制体上的脚本继承它, 生命周期由 UIManager 统一管理.
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    /// <summary>面板当前是否处于显示状态</summary>
    public bool IsShow { get; private set; }

    /// <summary>面板实例化后调用一次 (由 UIManager 调用)</summary>
    public virtual void OnInit() { }

    /// <summary>面板显示时调用</summary>
    public virtual void OnShow() { }

    /// <summary>面板隐藏时调用</summary>
    public virtual void OnHide() { }

    internal void Show()
    {
        IsShow = true;
        gameObject.SetActive(true);
        OnShow();
    }

    internal void Hide()
    {
        IsShow = false;
        gameObject.SetActive(false);
        OnHide();
    }
}
