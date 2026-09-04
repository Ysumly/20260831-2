using UnityEngine;

/// <summary>
/// 所有管理器的基类, 由 ManagerCenter 统一创建、存放和初始化.
/// 子类重写 OnInit 做初始化, 不要依赖 Awake (生命周期由中心接管).
/// </summary>
public abstract class BaseManager : MonoBehaviour
{
    /// <summary>是否已完成初始化</summary>
    public bool IsInited { get; private set; }

    /// <summary>初始化回调, 由 ManagerCenter 在管理器创建后调用一次</summary>
    protected virtual void OnInit() { }

    internal void DoInit()
    {
        if (IsInited) return;
        IsInited = true;
        OnInit();
    }
}
