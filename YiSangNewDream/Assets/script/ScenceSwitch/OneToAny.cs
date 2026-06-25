using UnityEngine;
using UnityEngine.SceneManagement;

public class OneToAny : MonoBehaviour
{
    public static OneToAny Instance;

    [Header("目标场景名")]
    public string targetSceneName;

    [Header("Resources 里的预制件路径，不要写 .prefab")]
    public string prefabPath;

    [Header("父物体名字")]
    public string parentName;

    // 当前生成出来的对象
    private GameObject spawnedObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            SpawnPrefabInTargetScene();
        }
        else
        {
            DestroySpawnedPrefab();
        }
    }

    public void SpawnPrefabInTargetScene()
    {
        if (spawnedObject != null)
            return;

        if (SceneManager.GetActiveScene().name != targetSceneName)
            return;

        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("prefabPath 为空，无法生成预制件");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError("没有找到预制件：" + prefabPath);
            return;
        }

        Transform parent = FindObjectInScene(parentName);

        if (parent == null)
        {
            Debug.LogError("没有找到父物体：" + parentName);
            return;
        }

        spawnedObject = Instantiate(prefab, parent);

        Debug.Log("已生成预制件：" + prefabPath + "，父物体：" + parentName);
    }

    public void DestroySpawnedPrefab()
    {
        if (spawnedObject == null)
            return;

        Destroy(spawnedObject);
        spawnedObject = null;

        Debug.Log("已销毁生成的预制件");
    }

    public void SetTarget(string sceneName, string resourcesPrefabPath, string targetParentName)
    {
        targetSceneName = sceneName;
        prefabPath = resourcesPrefabPath;
        parentName = targetParentName;
    }

    private Transform FindObjectInScene(string objectName)
    {
        if (string.IsNullOrEmpty(objectName))
        {
            Debug.LogError("parentName 为空，无法查找父物体");
            return null;
        }

        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform t in allTransforms)
        {
            if (t.name != objectName)
                continue;

            if (t.gameObject.scene == SceneManager.GetActiveScene())
                return t;
        }

        return null;
    }
}
//单例，并且设置场景名和预制件路径，然后写个函数用来再指定场景生成该预制件并写一个销毁预制件的函数（离开场景的时候用）
/*用法：
    OneToAny.Instance.SetTarget(
    "Middle",
    "character/Chapter1/S1/C2/Sinclair",
    "Canvas"
);
即可在外部脚本中直接生成*/