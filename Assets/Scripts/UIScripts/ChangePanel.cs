using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnLeftChange;
    [SerializeField] private Button btnRightChange;
    [SerializeField] private Button btnUnlockRole;
    [SerializeField] private TextMeshProUGUI MoneyNumber;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI PlayerMessage;
    [SerializeField] private TextMeshProUGUI NeedMoney;
    [SerializeField] private GameObject UnlockRolePanel;

    private int nowId;
    private int currentRoleIndex = 0;

    #region 稀疏组 以后有机会独立出去做成一个通用的稀疏集类, 现在先写在这里
    private int[] Ids;
    private List<GameObject> smallGameObjectPool;

    // 将小型GameObject插入稀疏集, 并克隆其本体到环境中
    private void insertSmallGameObjectPool(int index)
    {
        //Debug.Log(GameDataManager.Instance.RoleInfos[index].res);
        Ids[index] = smallGameObjectPool.Count;
        smallGameObjectPool.Add(Instantiate(
                Resources.Load<GameObject>(
                    ManagerCenter.Instance.Get<GameDataManager>().RoleInfos[index].res
                )
            )
        );
    }
    // 获取稀疏集中的小型GameObject
    private GameObject getSmallGameObjectPool(int index)
    {
        if (Ids[index] == -1)
        {
            insertSmallGameObjectPool(index);
        }
        return smallGameObjectPool[Ids[index]];
    }
    // 清空稀疏集, 释放内存
    private void clearSmallGameObjectPool()
    {
        for(int i = 0; i < smallGameObjectPool.Count; i++)
        {
            Ids[i] = -1;
            Destroy(smallGameObjectPool[i]);
        }
        smallGameObjectPool.Clear();
        
    }
    // 初始化稀疏集
    private void startSmallGameObjectPool(int lenth)
    {
        // 小型稀疏集
        Ids = new int[lenth];
        smallGameObjectPool = new List<GameObject>();
        for (int i = 0; i < Ids.Length; i++)
        {
            Ids[i] = -1;
        }
    }



    #endregion

    //当选角面板显示时, 将回调函数绑定到按钮上
    public override void OnInit()
    {
        base.OnInit();
        startSmallGameObjectPool(ManagerCenter.Instance.Get<GameDataManager>().RoleInfos.Length);
        getSmallGameObjectPool(currentRoleIndex);

        btnExit.onClick.AddListener(btnExitClick);
        btnStart.onClick.AddListener(btnStartClick);
        btnLeftChange.onClick.AddListener(btnLeftChangeClick);
        btnRightChange.onClick.AddListener(btnRightChangeClick);
        btnUnlockRole.onClick.AddListener(btnUnlockRoleClick);


    }



    private void btnExitClick()
    {
        ManagerCenter.Instance.Get<UIManager>().HidePanel<ChangePanel>();
        Camera.main.GetComponent<CameraRotation>().RightRotation(() =>
        {
            ManagerCenter.Instance.Get<UIManager>().ShowPanel<BeginPanel>();
        });
        getSmallGameObjectPool(currentRoleIndex).SetActive(false);
    }

    private void btnStartClick()
    {
        //判断是否解锁该角色, 如果没有解锁, 就提示解锁, 如果解锁了, 就进入游戏场景
        if(DoLock())
        {
            ManagerCenter.Instance.Get<UIManager>().ShowPanel<HintPanel>();
            return;
        }
        ManagerCenter.Instance.Get<UIManager>().HidePanel<ChangePanel>();
        getSmallGameObjectPool(currentRoleIndex).SetActive(false);
        // 打开选择面板
        ManagerCenter.Instance.Get<UIManager>().ShowPanel<SelectScenePanel>();
    }

    private void btnLeftChangeClick()
    {
        getSmallGameObjectPool(currentRoleIndex).SetActive(false);
        currentRoleIndex--;
        if (currentRoleIndex < 0)
        {
            currentRoleIndex = ManagerCenter.Instance.Get<GameDataManager>().RoleInfos.Length - 1;
        }
        getSmallGameObjectPool(currentRoleIndex).SetActive(true);
        DoLock();
        //处理文字信息
        ProcessText();

    }

    private void ProcessText()
    {
        PlayerName.text = ManagerCenter.Instance.Get<GameDataManager>().RoleInfos[currentRoleIndex].name;
        PlayerMessage.text = ManagerCenter.Instance.Get<GameDataManager>().RoleInfos[currentRoleIndex].tips;
    }

    private void btnRightChangeClick()
    {
        getSmallGameObjectPool(currentRoleIndex).SetActive(false);
        currentRoleIndex++;
        if(currentRoleIndex >= ManagerCenter.Instance.Get<GameDataManager>().RoleInfos.Length) {
            currentRoleIndex = 0;
        }
        getSmallGameObjectPool(currentRoleIndex).SetActive(true);
        DoLock();
        //处理文字信息
        ProcessText();

    }

    private void btnUnlockRoleClick()
    {
        // 解锁角色逻辑
        // 先判断是否已经解锁了, 是不是显示错误, 显示错误的话就更新一下UI即可
        if(DoLock())
        {
            // 检查金币是否足够
            int lockMoney = ManagerCenter.Instance.Get<GameDataManager>().RoleInfos[currentRoleIndex].lockMoney;
            if (ManagerCenter.Instance.Get<GameDataManager>().UserData.MoneyNumber >= lockMoney)
            {
                // 扣除金币
                ManagerCenter.Instance.Get<GameDataManager>().UserData.MoneyNumber -= lockMoney;

                // 解锁角色
                short x = 1;
                for (int i = 0; i < currentRoleIndex; i++)
                {
                    x *= 2;
                }
                ManagerCenter.Instance.Get<GameDataManager>().UserData.unlockedRoleNum |= x;

                // 更新UI
                MoneyNumber.text = ManagerCenter.Instance.Get<GameDataManager>().UserData.MoneyNumber.ToString();
                UnlockRolePanel.SetActive(false);

                ManagerCenter.Instance.Get<GameDataManager>().SaveUserData();
            }
        }
    }
    public override void OnShow()
    {
        base.OnShow();
        //显示剩余金钱
        MoneyNumber.text = ManagerCenter.Instance.Get<GameDataManager>().UserData.MoneyNumber.ToString();
        DoLock();
        ProcessText();
        getSmallGameObjectPool(currentRoleIndex).SetActive(true); 
    }
    private bool DoLock()
    {
        short x = 1;
        for (int i = 0; i < currentRoleIndex; i++)
        {
            x *= 2;
        }
        if ((ManagerCenter.Instance.Get<GameDataManager>().UserData.unlockedRoleNum & x) == 0)
        {
            // 角色未解锁
            UnlockRolePanel.SetActive(true);
            // 显示解锁所需的金币数
            NeedMoney.text = ManagerCenter.Instance.Get<GameDataManager>().RoleInfos[currentRoleIndex].lockMoney.ToString();
            return true;
        }
        else
        {
            // 角色已解锁
            UnlockRolePanel.SetActive(false);
            return false;
        }

    }
}
