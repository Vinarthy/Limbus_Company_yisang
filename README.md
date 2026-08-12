# Limbus Company · Yi Sang New Dream

一个使用 Unity 制作的《Limbus Company》李箱相关非商业同人游戏项目。

项目以视觉小说式剧情演出为主轴，并加入章节/日期推进、调饮与角色招待、自由经营、商店与收藏等玩法。目前仓库主要用于个人开发、工程备份和技术记录，内容仍在持续完善中。

> 本项目是非官方同人创作，与 Project Moon 无隶属或授权关系。原作角色、世界观及相关素材的权利归原权利方所有。

## 当前内容

- 视觉小说式剧情：JSON 台词、打字机文本、角色立绘与气泡演出、点击或空格推进
- 剧情进度：按章节、日期和场次加载内容，支持跨场景保存与继续
- 场景流程：开场、剧情生成、中转经营、室内/户外剧情及结尾场景
- 调饮与招待：物品拖拽、茶与调味材料、垃圾桶、上菜、角色检查和任务判定
- 自由经营：金币获取与消费、商店购买、商品状态持久化
- 收藏与个性化：背包、画廊解锁、场景装饰、可切换音乐
- 音频系统：主 BGM、临时 BGM、音效和 UI 音效，支持场景切换及淡入淡出
- 视觉表现：DOTween 动画、分辨率/正交相机适配、淡入淡出和回忆画面后处理
- 存档兼容：新版本会把新增的画廊、茶、调味材料和商店条目合并进旧存档

## 场景说明

当前 Build Settings 中启用的场景如下：

| 场景 | 用途 |
| --- | --- |
| `Start` | 游戏入口与开场流程 |
| `Before` | 根据存档进度生成章节/日期剧情 |
| `Middle` | 自由经营中转区域，承载材料、商店、背包和装饰等界面 |
| `League` | 剧情或玩法场景 |
| `Mountain` | 山区相关剧情或玩法场景 |
| `OutDoor` | 户外 CG 与角色对话衔接流程 |
| `ED` | 结尾流程 |

`Test.unity` 为开发测试场景，当前未加入正式构建列表。

## 主要系统

### 剧情与对话

剧情文本主要存放于 `Assets/Resources/Dialog`，通过 JSON 组织角色名、台词和行号等信息。`GalControl`、`Plot_Dy`、`Typewriter` 与相关演出脚本共同负责文本推进、逐字显示、立绘变化和剧情事件。

### 进度与存档

- `SaveData` 保存章节、日期和场次进度
- `StoryManager` 根据当前存档加载对应剧情节点并负责推进
- `StoryDatabase` 维护剧情节点与 Resources 预制体路径的映射
- 主进度存档位于 `Application.persistentDataPath`
- 收藏、材料和商店等扩展数据写入 `Other.json`
- `SaveTotalJson` 会以 `Resources/DefaultOther.json` 为基准，为旧存档补充新增条目，同时保留玩家已有状态

### 调饮、材料与任务

`Drink_Check` 目录包含调饮玩法的拖拽、物品属性、角色移动/检查、垃圾桶、上菜、引导及任务面板逻辑。茶与调味材料具有独立解锁状态，商店购买后可立即刷新经营场景中的材料显示。

### 金币、商店与背包

- `MoneyManage` 统一管理金币增减、持久化和 UI 通知
- `ShopManager` 读取商品数据、记录购买状态，并解锁材料或加入背包
- `BagManage` 展示已获得物品及详细信息
- 背包中的音乐可切换主 BGM，装饰物可应用到 `Middle` 场景

### 画廊与装饰

画廊按条目记录解锁状态并提供查看界面。装饰系统从存档读取当前选择，通过 Resources 加载对应预制体；切换后可在经营场景即时刷新。

### 音频与视觉效果

`AudioManager` 使用独立 AudioSource 管理主 BGM、临时 BGM、音效和 UI 音效，并提供音量控制与淡入淡出。项目同时使用 DOTween、Shader 与自定义脚本完成界面移动、缩放、物体渐隐渐现及回忆画面效果。

## 技术栈

- Unity `2022.3.53f1c1`
- C# / Unity 2D
- Universal Render Pipeline `14.0.11`
- UGUI / TextMeshPro `3.0.7`
- DOTween
- JSON / `JsonUtility`
- Resources 资源加载
- SceneManager 场景管理
- 2D Collider / Trigger

## 项目结构

```text
Assets/
├─ Plugins/Demigiant/DOTween/       # DOTween 插件
├─ Resources/
│  ├─ CGAndBackground/               # CG 与背景资源
│  ├─ character/                     # 角色与剧情预制体
│  ├─ Dialog/                        # JSON 剧情文本
│  ├─ Font/                          # 字体资源
│  ├─ Scene1/ Scene2/ Scene3/        # 各玩法与场景资源
│  └─ UI/                            # UI、背包、商店等资源
├─ Scenes/                           # 正式场景与测试场景
├─ script/
│  ├─ Audio/                         # BGM、音效与场景音乐
│  ├─ Drink_Check/                   # 调饮、角色招待和任务判定
│  ├─ Guide/                         # 开场引导
│  ├─ LoadAndRead/                   # 剧情、存档、商店、背包、画廊等
│  ├─ MoneySystem/                   # 金币系统与 UI
│  ├─ ScenceSwitch/                  # 场景切换与选择逻辑
│  ├─ TextManage/                    # 对话、打字机和角色演出
│  └─ VisualEffects/                 # 视觉效果
└─ Shaders/                          # 自定义 Shader
```

## 运行项目

1. 使用 Unity Hub 添加项目根目录。
2. 使用 Unity `2022.3.53f1c1` 打开工程；建议保持在 Unity 2022.3 LTS 系列。
3. 等待 Package Manager 完成依赖解析和资源导入。
4. 打开 `Assets/Scenes/Start.unity`。
5. 点击 Play，从正式入口体验当前流程。

首次导入耗时取决于本机性能。`Library`、`Temp`、`Logs` 等目录均可由 Unity 自动生成，不应作为项目内容手动维护。

## 开发提示

- 新增剧情时，需要同时准备 JSON 台词、剧情预制体，并更新 `StoryDatabase.cs` 中的节点映射。
- 新增商店、画廊、茶或调味材料条目时，应同步更新 `Resources/DefaultOther.json`；现有玩家的 `Other.json` 会在启动时增量补全。
- 需要在背包中使用的音乐或装饰，应配置正确的类型、名称和 Resources 路径。
- 正式流程从 `Start` 场景开始；直接打开中间场景调试时，需确认全局管理器与存档初始化对象已经存在。

## 当前状态

项目处于开发中，部分剧情、引导、UI 反馈与边缘流程仍可能调整。仓库内容以当前工程实际实现为准，README 会随主要系统更新。

