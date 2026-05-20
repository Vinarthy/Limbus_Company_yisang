using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    // 当前运行时剧情状态
    public SaveData currentData;

    // 当前剧情节点
    public StoryNode currentNode;

    // 当前实例化对象
    private GameObject currentSceneObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeStory();
    }

    // 游戏启动时初始化
    void InitializeStory()
    {
        currentData = SaveSystem.LoadGame();

        RefreshCurrentNode();

        SpawnCurrentScene();
    }

    // 刷新当前节点
    void RefreshCurrentNode()//同步savedata内容，方便该存储时存储
    {
        currentNode =
            StoryDatabase.Instance.GetStoryNode(
                currentData.chapter,
                currentData.day,
                currentData.scene
            );
    }

    // 删除原有预制件然后对照表加载新的
    void SpawnCurrentScene()
    {
        if (currentNode == null)
        {
            Debug.LogError("当前节点为空");
            return;
        }

        // 删除旧场景
        if (currentSceneObject != null)
        {
            Destroy(currentSceneObject);
        }

        GameObject prefab =
            Resources.Load<GameObject>(
                currentNode.prefabPath
            );

        if (prefab == null)
        {
            Debug.LogError(
                "Prefab不存在: " +
                currentNode.prefabPath
            );

            return;
        }

        currentSceneObject =
            Instantiate(prefab);
    }

    // 推进剧情
    public void NextScene()
    {
        currentData.scene++;//同步currentData内容

        RefreshCurrentNode();

        SpawnCurrentScene();
    }
    public void Nextday()
    {
        currentData.day++;//同步currentData内容

        RefreshCurrentNode();

        SpawnCurrentScene();
    }
    public void Nextchapter()
    {
        currentData.chapter++;//同步currentData内容

        RefreshCurrentNode();

        SpawnCurrentScene();
    }

    // 手动存档
    public void SaveGame()
    {
        SaveSystem.SaveGame(currentData);

        Debug.Log("游戏已保存");
    }
}
//想了想应该是存档是单例的，按返回的话清空原有角色并返回场景1，场景1通过bool来判断是否重新生成对象（场景切换时触发）
//特殊逻辑给角色弄
//StoryManage要单例模式，全局存在，存档模式这些都要单例吧卧槽