using System.IO;
using UnityEngine;

/// <summary>
/// 游戏数据管理器: 复用现有 JsonManager (JsonFileManager) 读写 JSON 存档.
/// 存档默认保存在 Application.persistentDataPath 下.
/// </summary>
public class GameDataManager : BaseManager<GameDataManager>
{
    private const string UserDataFileName = "UserData";
    public UserData UserData{ get; private set; }
    public RoleInfo[] RoleInfos { get; private set; }
    public SceneInfo[] SceneInfos { get; private set; }

    protected override void OnInit()
    {
        // 尝试加载用户数据, 如果不存在则创建默认数据
        UserData = LoadData<UserData>(UserDataFileName);
        if (UserData == null)
        {
            UserData = new UserData
            {
                MusicVolume = 0.5f,
                SFXVolume = 0.5f,
                MoneyNumber = 0
            };
            SaveData(UserData, UserDataFileName);
        }
        //Debug.Log(UserData);

        // 尝试加载角色数据
        RoleInfos = LoadData<RoleInfo[]>("RoleInfo");
        if (RoleInfos == null)
        {
            Debug.LogError("角色数据加载失败, 请确保 RoleInfo.json 文件存在于 Application.streamingAssetsPath 下."); return;
        }

        // 尝试加载场景数据
        SceneInfos = LoadData<SceneInfo[]>("SceneInfo");
        if (SceneInfos == null)
        {
            Debug.LogError("场景数据加载失败, 请确保 SceneInfo.json 文件存在于 Application.streamingAssetsPath 下."); return;
        }
        }

    /// <summary>
    /// 保存用户数据到 JSON 文件
    /// </summary>
    public void SaveUserData()
    {
        SaveData(UserData, UserDataFileName);
    }



    /// <summary>保存数据到 JSON 文件 (默认使用 LitJson)</summary>
    public void SaveData(object data, string fileName)
    {
        JsonFileManager.Instance.Save(data, fileName);
    }

    /// <summary>从 JSON 文件读取数据, 文件不存在时返回 default(T)</summary>
    public T LoadData<T>(string fileName)
    {
        return JsonFileManager.Instance.Load<T>(fileName);
    }

    /// <summary>删除指定存档</summary>
    public void DeleteData(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (File.Exists(path))
            File.Delete(path);
    }
}
