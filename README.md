# 20260831-2

Unity 游戏项目。采用自研 **ManagerOfManagers（管理器中心）** 框架，当前已完成框架基础搭建与 **BeginScene**（开始场景 / 选角选关）的搭建。

## 已完成内容

### 1. ManagerOfManagers 基础框架设计

框架核心是全局唯一、常驻不销毁的 `ManagerCenter`（管理器中心），统一负责创建、存放和初始化所有管理器。

```
Assets/Scripts/ManagerFramework/
├── ManagerCenter.cs          # 管理器中心 (Manager of Managers)
├── BaseManager.cs            # 所有管理器基类
├── BaseManagerT.cs           # 泛型单例管理器基类
├── Audio/AudioManager.cs     # 音频管理器 (背景音乐 / 音效)
├── Data/GameDataManager.cs   # 游戏数据管理器 (JSON 存档读写)
├── Level/GameLevelManager.cs # 关卡管理器 (场景加载 / 切换)
└── UI/
    ├── UIManager.cs          # UI 管理器 (面板预制体管理)
    └── BasePanel.cs          # UI 面板基类
```

**设计要点**

- `ManagerCenter` 通过 `RuntimeInitializeOnLoadMethod` 在场景加载前自动创建，运行时随时可用。
- 子管理器通过 `ManagerCenter.Instance.Get<T>()` 或 `BaseManager<T>.Instance` 获取，首次访问时自动创建并调用 `OnInit` 初始化。
- `BaseManager` 统一接管初始化生命周期（`OnInit`），子类不依赖 `Awake`。
- `UIManager` 约定面板预制体放在 `Resources/UI/` 下、预制体名与面板类型名一致，通过 `ShowPanel<T>()` / `HidePanel<T>()` 显示与隐藏，面板隐藏后缓存复用。

### 2. 稀疏组（Sparse Set）实现

在选角面板 `ChangePanel` 中实现了一个小型稀疏组，用于按需加载 / 缓存角色模型 GameObject。

- 用 `int[] Ids` 记录「角色下标 → 对象池下标」的映射，`-1` 表示尚未加载。
- 首次访问时按需 `Instantiate` 角色资源并缓存，再次访问直接复用。
- 提供 `clearSmallGameObjectPool()` 清空并释放内存。

> 该稀疏组目前内嵌于 `ChangePanel` 中，代码注释里已预留「以后独立成通用稀疏集类」的规划。

### 3. 其他

- **BeginScene 搭建**：`BeginScenesMain` 作为开始场景入口，启动时展示 `BeginPanel` 并初始化数据管理器、播放背景音乐。
- **UI 面板流**：`BeginPanel`（开始）→ `ChangePanel`（选角）→ `SelectScenePanel`（选关），另有 `SettingPanel` / `HintPanel`。
- **镜头过渡**：`CameraRotation` 实现开始 / 选角之间的镜头旋转过渡。
- **JSON 存档系统**：`JsonManager`（含 `LitJson`）提供统一读写接口，`GameDataManager` 复用其读写 `UserData` / `RoleInfo` / `SceneInfo` 存档。
- **数据模型**：`UserData`（音量、金币、角色解锁位标记）、`RoleInfo`（角色信息）、`SceneInfo`（场景信息）。

## 目录结构

```
Assets/
├── ArtRes/                # 美术资源 (第三方资源包)
├── Scenes/
│   ├── BeginScene.unity   # 开始场景
│   └── GameScene.unity    # 游戏场景
└── Scripts/
    ├── Camera/            # 相机控制
    ├── Data/              # 数据模型 (UserData / RoleInfo / SceneInfo)
    ├── JsonManager/       # JSON 读写 (LitJson)
    ├── Main/              # 场景入口 (BeginScenesMain)
    ├── ManagerFramework/  # 管理器框架
    ├── TestFiles/         # 测试脚本
    └── UIScripts/         # UI 面板脚本
```

## 运行环境

- Unity（脚本基于 `UnityEngine` / `TMPro` / `UI`，使用 `FindFirstObjectByType` 等较新 API）
- 依赖 LitJson（已内置于 `Assets/Scripts/JsonManager/LitJson/`）
