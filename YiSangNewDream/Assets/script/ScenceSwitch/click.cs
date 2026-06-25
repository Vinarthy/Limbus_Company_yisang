using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//思路：设置预制件，设置场景名，设置传送坐标,并在检测到场景2的时候实例化对应的预制件
//搞个单例挂上去得了吧，实时记录，ok

public class click : MonoBehaviour
{
    public static click Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<GameObject> characters = new List<GameObject>(); // 角色列表
    private List<GameObject> spawnedCharacters = new List<GameObject>(); // 记录已生成的角色，方便后续管理
    private string SceneName="Middle";//目标场景名
    private bool hasSpawnedInMiddle = false; // 防止重复实例化
    private string currentSceneName; // 记录当前场景名

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        Debug.Log("场景已加载: " + currentSceneName);

        // 检查是否是目标场景
        if (currentSceneName == SceneName)
        {
            SpawnCharactersInMiddle();
        }
        else
        {
            hasSpawnedInMiddle = false;
        }
    }

    // 在Middle场景中实例化角色
    void SpawnCharactersInMiddle()
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogWarning("角色预制件列表为空!");
            return;
        }

        if (!hasSpawnedInMiddle)
        {
            foreach (var prefab in characters)
            {
                if (prefab != null)
                {
                    GameObject spawned = Instantiate(prefab);
                    spawnedCharacters.Add(spawned); // 记录引用
                    Debug.Log("已实例化角色: " + prefab.name);
                }
                else
                {
                    Debug.LogWarning("列表中存在空的预制件引用，已跳过");
                }
            }
            hasSpawnedInMiddle = true;
            Debug.Log($"已在Middle场景实例化 {spawnedCharacters.Count} 个角色");
        }
    }
    public void ClearSpawnedCharacters()
    {
        foreach (GameObject obj in spawnedCharacters)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        spawnedCharacters.Clear();

        hasSpawnedInMiddle = false;

        Debug.Log("已清除所有实例化角色");//清除
    }

}
//为什么实例化两个角色了？