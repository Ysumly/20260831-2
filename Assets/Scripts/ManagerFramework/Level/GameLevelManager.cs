using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 关卡管理器: 管理关卡场景列表与场景加载/切换.
/// 先通过 SetLevels 配置关卡场景名列表, 再用 LoadLevel / LoadNextLevel / RestartLevel 切换.
/// </summary>
public class GameLevelManager : BaseManager<GameLevelManager>
{
    private readonly List<string> levelNames = new List<string>();

    /// <summary>关卡总数</summary>
    public int LevelCount => levelNames.Count;

    /// <summary>当前关卡下标 (未加载任何关卡时为 0)</summary>
    public int CurrentLevelIndex { get; private set; }

    /// <summary>当前关卡场景名 (未配置关卡时为空字符串)</summary>
    public string CurrentLevelName => IsValidIndex(CurrentLevelIndex) ? levelNames[CurrentLevelIndex] : string.Empty;

    /// <summary>设置关卡场景名列表 (场景需已加入 Build Settings)</summary>
    public void SetLevels(IEnumerable<string> names)
    {
        levelNames.Clear();
        if (names != null)
            levelNames.AddRange(names);
    }

    /// <summary>下标是否对应一个有效关卡</summary>
    public bool IsValidIndex(int index) => index >= 0 && index < levelNames.Count;

    /// <summary>加载指定下标的关卡场景</summary>
    public void LoadLevel(int index)
    {
        if (!IsValidIndex(index))
        {
            Debug.LogWarning($"[GameLevelManager] 无效的关卡下标: {index}");
            return;
        }
        CurrentLevelIndex = index;
        SceneManager.LoadScene(levelNames[index]);
    }

    /// <summary>按场景名加载关卡</summary>
    public void LoadLevel(string levelName)
    {
        int index = levelNames.IndexOf(levelName);
        if (index < 0)
        {
            Debug.LogWarning($"[GameLevelManager] 关卡不在列表中: {levelName}");
            return;
        }
        LoadLevel(index);
    }

    /// <summary>加载下一关 (已是最后一关时不生效并给出警告)</summary>
    public void LoadNextLevel() => LoadLevel(CurrentLevelIndex + 1);

    /// <summary>重新加载当前关</summary>
    public void RestartLevel() => LoadLevel(CurrentLevelIndex);
}
