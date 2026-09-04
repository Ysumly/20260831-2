using UnityEngine;

public class BeginScenesMain : MonoBehaviour
{
    private void Start()
    {
        ManagerCenter.Instance.Get<UIManager>().ShowPanel<BeginPanel>();
        //初始化游戏数据管理器
        ManagerCenter.Instance.Get<GameDataManager>();
        //获取音乐, 然后播放背景音乐
        AudioClip musicClip = Resources.Load<AudioClip>("Audio/New Hope_Drums Only");
        ManagerCenter.Instance.Get<AudioManager>().PlayMusic(musicClip);
    }
}
