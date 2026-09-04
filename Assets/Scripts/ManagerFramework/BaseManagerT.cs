using UnityEngine;

/// <summary>
/// 泛型单例管理器基类.
/// 首次访问 Instance 时通过 ManagerCenter 自动创建, 全局唯一.
/// 子类若重写 OnDestroy, 需调用 base.OnDestroy().
/// </summary>
public abstract class BaseManager<T> : BaseManager where T : BaseManager<T>
{
    private static T instance;

    /// <summary>管理器单例, 首次访问时自动创建</summary>
    protected static T Instance
    {
        get
        {
            if (instance == null)
                instance = ManagerCenter.Instance.Get<T>();
            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    } 
}
