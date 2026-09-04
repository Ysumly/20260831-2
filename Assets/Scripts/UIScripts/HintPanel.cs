using UnityEngine;
using UnityEngine.UI;

public class HintPanel : BasePanel
{
    [SerializeField] private Button btnExit;


    public override void OnInit()
    {
        base.OnInit();
        btnExit.onClick.AddListener(() =>
        {
            ManagerCenter.Instance.Get<UIManager>().HidePanel<HintPanel>();
        });
    }

    public override void OnShow()
    {
        base.OnShow();
        // 播放个音效
        AudioClip sfxClip = Resources.Load<AudioClip>("Audio/Zombie Sounds/Zombie Hit_05");
        ManagerCenter.Instance.Get<AudioManager>().PlaySfx(sfxClip);
    }
}





