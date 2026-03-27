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

    public GameObject character;//此为设置的预制件，记得挂载到相关角色上面去
    private GameObject Temp;
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
            SpawnCharacterInMiddle();
        }
        else
        {
            hasSpawnedInMiddle = false;
        }
    }

    // 在Middle场景中实例化角色
    void SpawnCharacterInMiddle()
    {
        if (character != null && !hasSpawnedInMiddle)
        {
            Temp=Instantiate(character);
            hasSpawnedInMiddle = true;
            Debug.Log("已在Middle场景实例化角色");
        }
        else if (character == null)
        {
            Debug.LogWarning("角色预制件未分配!");
        }
    }


}
