using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public SaveData currentData;

    private GameObject currentStoryObject;
    private GalControl currentGalControl;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentData = SaveSystem.LoadGame();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnsubscribeCurrentGal();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Before")
            SpawnCurrentStory();
        else if (scene.name == "Middle")
            ClearCurrentStory();
    }

    public StoryNode GetCurrentNode()
    {
        return StoryDatabase.Instance.GetStoryNode(
            currentData.chapter,
            currentData.day,
            currentData.scene
        );
    }

    private void SpawnCurrentStory()
    {
        StoryNode node = GetCurrentNode();
        if (node == null)
        {
            Debug.LogError("当前剧情节点为空");
            return;
        }

        ClearCurrentStory();

        GameObject prefab = Resources.Load<GameObject>(node.prefabPath);
        if (prefab == null)
        {
            Debug.LogError("找不到预制件：" + node.prefabPath);
            return;
        }

        if (IsOpeningGalNode())
        {
            Transform galParent = FindOpeningGalParent();
            if (galParent == null)
            {
                Debug.LogError("未找到 Before/Canvas/Gal，无法生成开场剧情");
                return;
            }

            currentStoryObject = Instantiate(prefab, galParent, false);
            currentGalControl = currentStoryObject.GetComponentInChildren<GalControl>(true);

            if (currentGalControl == null)
            {
                Debug.LogError("1-1-1 开场预制件缺少 GalControl", currentStoryObject);
            }
            else
            {
                currentGalControl.PlaybackCompleted += OnOpeningGalCompleted;
            }
        }
        else
        {
            currentStoryObject = Instantiate(prefab);
        }

        Debug.Log($"生成剧情对象：{node.prefabPath}");
    }

    private void ClearCurrentStory()
    {
        UnsubscribeCurrentGal();

        if (currentStoryObject == null)
            return;

        Destroy(currentStoryObject);
        currentStoryObject = null;
    }

    private bool IsOpeningGalNode()
    {
        return currentData.chapter == 1
            && currentData.day == 0
            && currentData.scene == 1;
    }

    private static Transform FindOpeningGalParent()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        return canvasObject != null ? canvasObject.transform.Find("Gal") : null;
    }

    private void OnOpeningGalCompleted()
    {
        UnsubscribeCurrentGal();

        if (currentStoryObject != null)
        {
            Destroy(currentStoryObject);
            currentStoryObject = null;
        }

        Nextday();
        SaveGame();
        SpawnCurrentStory();
    }

    private void UnsubscribeCurrentGal()
    {
        if (currentGalControl != null)
            currentGalControl.PlaybackCompleted -= OnOpeningGalCompleted;

        currentGalControl = null;
    }

    public void NextScene()
    {
        currentData.scene++;
    }

    public void Nextday()
    {
        currentData.day++;
        currentData.scene = 1;
    }

    public void Nextchapter()
    {
        currentData.chapter++;
        currentData.day++;
        currentData.scene = 1;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(currentData);
    }
}
