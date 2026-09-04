using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LitJsonHandle : IJsonFileHandle
{
    /// <summary>
    /// 用LitJson读取数据
    /// </summary>
    /// <typeparam name="T">读取类型</typeparam>
    /// <param name="path">读取路径</param>
    /// <returns></returns>
    public T Load<T>(string path)
    {
        return JsonMapper.ToObject<T>(File.ReadAllText(path));
    }
    /// <summary>
    /// 用LitJson保存数据
    /// </summary>
    /// <param name="data">保存的数据</param>
    /// <param name="path">保存路径</param>
    public void Save(object data, string path)
    {
        File.WriteAllText(path, JsonMapper.ToJson(data));
    }

    
}
