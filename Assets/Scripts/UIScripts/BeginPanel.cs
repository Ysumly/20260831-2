using System;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnAudio;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnAbout;
    
    public override void OnInit()
    {
        
        btnStart.onClick.AddListener(OnStartClick);
        btnAudio.onClick.AddListener(OnAudioClick);
        btnExit.onClick.AddListener(OnExitClick);
        btnAbout.onClick.AddListener(OnAboutClick);
    }


    //以下是按钮点击事件的处理方法
    //每个方法中可以添加具体的逻辑，比如切换场景、打开设置面板等
    //目前只是简单的打印日志，表示按钮被点击了
    //记得在实际项目中根据需求实现具体的功能, 而且要Hide当前面板, 否则会遮挡其他UI
    private void OnAboutClick()
    {
        //throw new NotImplementedException();
        Debug.Log("About button clicked");
    }

    private void OnExitClick()
    {
        //游戏退出逻辑, 在编辑器中不会退出, 只会打印日志
        Application.Quit();
        Debug.Log("Exit button clicked");
    }

    private void OnAudioClick()
    {
        ManagerCenter.Instance.Get<UIManager>().ShowPanel<SettingPanel>();
    }

    private void OnStartClick()
    {
        //关闭当前面板
        ManagerCenter.Instance.Get<UIManager>().HidePanel<BeginPanel>();
        //旋转视野
        Camera.main.GetComponent<CameraRotation>().LeftRotation(() =>
        {
            //旋转结束后, 打开选择角色面板
            ManagerCenter.Instance.Get<UIManager>().ShowPanel<ChangePanel>();
        });
    }
}

