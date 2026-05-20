using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayRecord : MonoBehaviour
{
    // Start is called before the first frame update
    public static StoryManager Instance;
    public TimeBool timebool;
    public StoryManager storymanage;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 取消注册
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 场景加载完成后触发
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("进入场景：" + scene.name);

        if (scene.name == "Before")
        {
            Debug.Log("进入Before场景");
            if (timebool.DayAdd == true)
            {
                storymanage.Nextday();
                timebool.DayAdd = false;
            }
            else if (timebool.ChapterAdd == true)
            {
                storymanage.Nextchapter();
                timebool.ChapterAdd = false;
            }
            else if (timebool.SceneAdd == true)
            {
                storymanage.Nextchapter();
                timebool.SceneAdd = false;
            }
        }
    }

    void InitializeStory()
    {
        Debug.Log("初始化剧情");
    }
}
//场景切换时触发同步日期