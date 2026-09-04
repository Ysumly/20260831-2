using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理器中心 (Manager of Managers).
/// 全局唯一、常驻不销毁, 负责统一创建、存放和初始化所有 BaseManager.
/// 子管理器通过 ManagerCenter.Instance.Get&lt;T&gt;() 或 BaseManager&lt;T&gt;.Instance 获取.
/// </summary>
public class ManagerCenter : MonoBehaviour
{
    private static ManagerCenter instance;

    /// <summary>管理器中心单例, 不存在时自动创建</summary>
    public static ManagerCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<ManagerCenter>();
                if (instance == null)
                {
                    var go = new GameObject("ManagerCenter");
                    instance = go.AddComponent<ManagerCenter>();
                }
            }
            return instance;
        }
    }


    private readonly Dictionary<Type, BaseManager> managers = new Dictionary<Type, BaseManager>();

    /// <summary>游戏启动、场景加载前自动创建, 保证运行时随时可用</summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate() => _ = Instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // 场景中已存在中心时, 销毁重复实例
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>获取指定类型的管理器, 不存在时自动创建并初始化</summary>
    public T Get<T>() where T : BaseManager
    {
        if (managers.TryGetValue(typeof(T), out var manager) && manager != null)
            return (T)manager;

        // 每个管理器挂在中心下面的独立子物体上
        var child = new GameObject(typeof(T).Name);
        child.transform.SetParent(transform, false);
        T newManager = child.AddComponent<T>();
        managers[typeof(T)] = newManager;
        newManager.DoInit();
        return newManager;
    }

    /// <summary>获取已存在的管理器, 不存在时返回 null (不会自动创建)</summary>
    public T GetIfExists<T>() where T : BaseManager
    {
        return managers.TryGetValue(typeof(T), out var manager) && manager != null ? (T)manager : null;
    }
}
