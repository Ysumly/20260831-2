using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJsonFileHandle
{
    public void Save(object data,string path);
    public T Load<T>(string path);
}
