using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneSwitchBGM : MonoBehaviour
{
    public const string DefaultMainBgmId = "MainBGM";

    [System.Serializable]
    public class SceneBgmSetting
    {
        public string sceneName;
        public string secondaryBgmId;
    }

    public static SceneSwitchBGM Instance;

    [Header("AudioConfig 中对应的 ID")]
    public string mainBgmId = DefaultMainBgmId;

    [FormerlySerializedAs("secondaryBgmId")]
    public string defaultSecondaryBgmId = "显然，我不知道";//其实就是没有木哈哈哈哈哈

    [Header("其他场景的副 BGM")]
    public List<SceneBgmSetting> sceneBgmSettings = new List<SceneBgmSetting>();

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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start()
    {
        ApplySceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        Instance = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneMusic(scene.name);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopSecondaryBGM();
    }

    private void ApplySceneMusic(string sceneName)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("SceneSwitchBGM：场景中不存在 AudioManager。", this);
            return;
        }

        AudioManager.Instance.StopSecondaryBGM();

        if (UsesMainBGM(sceneName))
        {
            if (!string.IsNullOrWhiteSpace(mainBgmId))
                AudioManager.Instance.PlayBGM(mainBgmId);

            AudioManager.Instance.ResumeBGM();
            return;
        }

        AudioManager.Instance.PauseBGM();

        string targetBgmId = GetSecondaryBgmId(sceneName);
        if (!string.IsNullOrWhiteSpace(targetBgmId))
        {
            AudioManager.Instance.PlaySecondaryBGM(targetBgmId);
        }
        else
        {
            Debug.LogWarning($"SceneSwitchBGM：场景 {sceneName} 没有配置副 BGM。", this);
        }
    }

    public void SetMainBGM(string bgmId)
    {
        if (string.IsNullOrWhiteSpace(bgmId))
            return;

        mainBgmId = bgmId;

        if (AudioManager.Instance == null || !UsesMainBGM(SceneManager.GetActiveScene().name))
            return;

        AudioManager.Instance.PlayBGM(mainBgmId);
        AudioManager.Instance.ResumeBGM();
    }
    private string GetSecondaryBgmId(string sceneName)
    {
        foreach (SceneBgmSetting setting in sceneBgmSettings)
        {
            if (setting != null && setting.sceneName == sceneName)
                return setting.secondaryBgmId;
        }

        return defaultSecondaryBgmId;
    }

    private static bool UsesMainBGM(string sceneName)
    {
        return sceneName == "Start"
            || sceneName == "Before"
            || sceneName == "Middle";
    }
}