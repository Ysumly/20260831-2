using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class JsonFileManager
{
    private string persistentDataPath = Application.persistentDataPath;
    private string streamingAssetsPath = Application.streamingAssetsPath;
    //单例模式, 可以直接调用,不需要新建
    private static JsonFileManager instance = new JsonFileManager();
    public static JsonFileManager Instance { get { return instance; } }

    private JsonFileManager() { }

    /// <summary>
    /// 按照LitJson保存数据
    /// </summary>
    /// <param name="data">需要保存的数据</param>
    /// <param name="Filename">保存为的名字</param>
    public void Save(object data, string Filename)
    {
        Save(data, Filename, new LitJsonHandle());
    }

    /// <summary>
    /// 按照LitJson读取数据, 读取时会先查找只读文件夹
    /// </summary>
    /// <typeparam name="T">读取数据类型</typeparam>
    /// <param name="Filename">文件名</param>
    /// <returns></returns>
    public T Load<T>(string Filename)
    {
        return Load<T>(Filename, new LitJsonHandle());
    }

    /// <summary>
    /// 按照IJsonFileHandle保存数据
    /// </summary>
    /// <param name="data">需要保存的数据</param>
    /// <param name="Filename">需要保存的数据的名称</param>
    /// <param name="handle">调用的保存方法</param>
    public void Save(object data, string Filename, IJsonFileHandle handle)
    {
        if (handle == null)
        {
            throw new System.Exception("Saving : IJsonFileHandle is null");
        }
        if (string.IsNullOrEmpty(Filename))
        {
            throw new System.Exception("Saving : Filename is null");
        }
        handle.Save(data, persistentDataPath + "/" + Filename + ".json");
    }

    /// <summary>
    /// 按照IJsonFileHandle读取数据, 读取时会先查找只读文件夹
    /// </summary>
    /// <typeparam name="T">返回读取到的对象</typeparam>
    /// <param name="Filename">需要读取的名字</param>
    /// <param name="handle">调用读取的办法</param>
    /// <returns></returns>
    public T Load<T>(string Filename, IJsonFileHandle handle)
    {
        if (handle == null)
        {
            throw new System.Exception("Loading : IJsonFileHandle is null");
        }
        if (string.IsNullOrEmpty(Filename))
        {
            throw new System.Exception("Loading : Filename is null");
        }
        string fullPath = streamingAssetsPath + "/" + Filename + ".json";
        if (!File.Exists(fullPath))
        {
            fullPath = persistentDataPath + "/" + Filename + ".json";
        }
        if (!File.Exists(fullPath))
            return default;
        return handle.Load<T>(fullPath);
    }
}
