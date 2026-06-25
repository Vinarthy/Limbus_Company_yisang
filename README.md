# Limbus_Company_yisang

一个基于 Unity 开发的《Limbus Company》相关同人游戏项目备份。项目以异想体/角色剧情演出为核心，包含视觉小说式对话、章节推进、存档读取、场景切换、2D 交互小游戏与基础视觉特效等内容。

> 本项目为个人学习与同人创作用途，主要用于项目备份、开发记录和技术展示。

## 项目简介

本项目使用 Unity 2D 制作，围绕角色剧情展开。游戏通过 JSON 配置剧情文本，使用 Unity 预制体组织不同章节、天数与场景内容，并通过脚本控制剧情播放、角色演出、任务判定和玩家交互。

当前项目包含多个场景：

- `Before`：剧情播放与章节内容生成场景
- `Middle`：中转/流程控制场景
- `League`：功能或剧情相关场景
- `Mountain`：功能或剧情相关场景

## 主要功能

- 剧情对话系统
  - 支持从 `Resources/Dialog` 读取 JSON 剧情文本
  - 支持角色名、台词内容和剧情行号管理
  - 支持鼠标点击或空格推进剧情

- 打字机文本效果
  - 基于 TextMeshPro 实现逐字显示
  - 支持点击时立即补全文本
  - 支持剧情播放完成事件回调

- 章节与进度管理
  - 使用章节、天数、场景编号管理剧情节点
  - 根据当前进度动态加载对应剧情预制体
  - 支持跨场景保留剧情管理对象

- 本地存档系统
  - 使用 `JsonUtility` 序列化存档数据
  - 使用 `Application.persistentDataPath` 保存本地 `save.json`
  - 支持无存档时自动创建默认进度

- 2D 交互小游戏
  - 支持鼠标拖拽物体
  - 支持 2D 碰撞/触发检测
  - 支持任务条件判断、角色检查、物品归位与完成判定

- 动效与视觉表现
  - 集成 DOTween，用于缩放、移动等交互反馈动画
  - 支持相机比例适配，自动调整窗口与正交相机尺寸
  - 支持回忆画面等屏幕后处理效果

## 技术栈

- Unity `2022.3.53f1c1`
- C#
- Unity 2D
- UGUI
- TextMeshPro
- DOTween
- JSON / `JsonUtility`
- Unity Resources 资源加载
- Unity SceneManager 场景管理
- 2D Collider / Trigger 交互

## 项目结构

```text
Assets/
├── Plugins/
│   └── Demigiant/DOTween/        # DOTween 插件
├── Resources/
│   ├── CGAndBackground/          # CG 与背景资源
│   ├── character/                # 角色与章节预制体资源
│   ├── Dialog/                   # 剧情 JSON 文本
│   ├── Font/                     # 字体与字符集资源
│   ├── Scene1/                   # 场景资源
│   ├── Scene2/                   # 场景资源
│   ├── Scene3/                   # 调饮/交互相关资源
│   └── UI/                       # UI 资源
├── Scenes/
│   ├── Before.unity
│   ├── Middle.unity
│   ├── League.unity
│   └── Mountain.unity
├── script/
│   ├── Drink_Check/              # 调饮/拖拽/任务检查玩法
│   ├── LoadAndRead/              # 存档、剧情节点、剧情管理
│   ├── ScenceSwitch/             # 场景切换与选择逻辑
│   ├── TextManage/               # 对话、打字机、角色演出
│   └── VisualEffects/            # 视觉效果
└── Shaders/                      # Shader 与屏幕效果
```

## 核心脚本说明

- `StoryManager.cs`：负责读取当前存档进度、加载剧情节点、生成剧情预制体并推进章节/天数/场景。
- `StoryDatabase.cs`：维护章节、天数、场景编号与 Resources 预制体路径的映射。
- `SaveSystem.cs`：负责本地存档保存与读取。
- `GalControl.cs`：负责读取 JSON 对话、推进文本、触发每行剧情事件和播放完成回调。
- `Typewriter.cs`：负责 TextMeshPro 文本打字机效果。
- `Drag.cs`：负责 2D 物体拖拽、归位和触发器交互。
- `CameraAspectFitter.cs`：负责窗口分辨率与正交相机画面适配。
- `MemoryRecallScreenEffect.cs`：负责回忆画面屏幕后处理效果。

## 运行方式

1. 使用 Unity Hub 打开项目根目录。
2. 推荐使用 Unity `2022.3.53f1c1` 或相近的 Unity 2022.3 LTS 版本。
3. 等待 Unity 导入资源与生成 Library。
4. 打开 `Assets/Scenes/Before.unity`。
5. 点击 Play 运行项目。

## 构建场景

项目当前构建列表包含：

```text
Assets/Scenes/Before.unity
Assets/Scenes/Middle.unity
Assets/Scenes/League.unity
Assets/Scenes/Mountain.unity
```

## 开发说明

- 剧情文本主要放置在 `Assets/Resources/Dialog` 下，以 JSON 文件组织。
- 剧情预制体主要放置在 `Assets/Resources/character/Chapter1Final` 下。
- 新增剧情节点时，需要在 `StoryDatabase.cs` 中添加章节、天数、场景编号与预制体路径映射。
- 存档文件运行后会生成在 Unity 的 `Application.persistentDataPath` 目录中。

## 版权声明

本项目为《Limbus Company》相关同人项目，仅用于学习、交流与个人备份，不用于商业用途。原作及相关角色、设定、素材版权归其原权利方所有。

