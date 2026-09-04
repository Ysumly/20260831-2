using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectScenePanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnLeftChange;
    [SerializeField] private Button btnRightChange;
    [SerializeField] private TextMeshProUGUI sceneName;
    [SerializeField] private TextMeshProUGUI sceneTipe;
    [SerializeField] private Image sceneImage;

    // 当前选择的场景索引
    [SerializeField] private int imageId;




    public override void OnInit()
    {
        base.OnInit();
        btnStart.onClick.AddListener(OnStartClick);
        btnExit.onClick.AddListener(OnExitClick);
        btnLeftChange.onClick.AddListener(OnLeftChangeClick);
        btnRightChange.onClick.AddListener(OnRightChangeClick);
    }

    public override void OnShow()
    {
        sceneName.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].name;
        sceneTipe.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].tips;
        sceneImage.sprite = Resources.Load<Sprite>(ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].imgRes);
    }


    private void OnRightChangeClick()
    {
        imageId++;
        if(imageId >= ManagerCenter.Instance.Get<GameDataManager>().SceneInfos.Length)
        {
            imageId = 0;
        }
        sceneName.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].name;
        sceneTipe.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].tips;
        sceneImage.sprite = Resources.Load<Sprite>(ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].imgRes);
    }

    private void OnLeftChangeClick()
    {
        imageId--;
        if(imageId < 0)
        {
            imageId = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos.Length - 1;
        }
        sceneName.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].name;
        sceneTipe.text = ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].tips;
        sceneImage.sprite = Resources.Load<Sprite>(ManagerCenter.Instance.Get<GameDataManager>().SceneInfos[imageId].imgRes);
    }

    private void OnExitClick()
    {
        ManagerCenter.Instance.Get<UIManager>().ShowPanel<ChangePanel>();
        ManagerCenter.Instance.Get<UIManager>().HidePanel<SelectScenePanel>();
    }

    private void OnStartClick()
    {
        Debug.Log("开始游戏"); 
    }




}

