using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtlityHandle : IJsonFileHandle
{
    /// <summary>
    /// 用JsonUtlity的方式读取数据
    /// </summary>
    /// <typeparam name="T">读取数据类型</typeparam>
    /// <param name="path">读取路径</param>
    /// <returns>读得数据</returns>
    /// <exception cref="System.Exception">读取路径不存在</exception>
    public T Load<T>(string path)
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }
    /// <summary>
    /// 用JsonUtlity的方式保存数据
    /// </summary>
    /// <param name="data">需保存的数据</param>
    /// <param name="path">需保存的路径</param>
    public void Save(object data, string path)
    {
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }
}
