using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [SerializeField] private Scrollbar sbrMusicVolume;
    [SerializeField] private Scrollbar sbrSfxVolume;
    [SerializeField] private Button btnExit;

    override public void OnInit()
    {
        sbrMusicVolume.onValueChanged.AddListener(OnMusicVolumeChange);
        sbrSfxVolume.onValueChanged.AddListener(OnSfxVolumeChange);
        btnExit.onClick.AddListener(OnExitClick);
    }

    public override void OnShow()
    {
        sbrMusicVolume.value = ManagerCenter.Instance.Get<AudioManager>().MusicVolume;
        sbrSfxVolume.value = ManagerCenter.Instance.Get<AudioManager>().SfxVolume;
    }

    public override void OnHide()
    {
        ManagerCenter.Instance.Get<GameDataManager>().SaveUserData();
    }

    private void OnExitClick()
    {
        // Hide the setting panel
        ManagerCenter.Instance.Get<UIManager>().HidePanel<SettingPanel>();
    }

    private void OnSfxVolumeChange(float arg0)
    {
        ManagerCenter.Instance.Get<AudioManager>().SetSfxVolume(arg0);
    }

    private void OnMusicVolumeChange(float arg0)
    {
        ManagerCenter.Instance.Get<AudioManager>().SetMusicVolume(arg0);
    }
}

