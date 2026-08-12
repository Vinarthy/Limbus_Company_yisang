using UnityEngine;
using UnityEngine.SceneManagement;

public class GalDayAdvanceOnComplete : MonoBehaviour
{
    public enum AdvanceType
    {
        Day,
        Scene,
        Chapter
    }

    [SerializeField] private string targetScene = "Before";
    [SerializeField] private AdvanceType advanceType = AdvanceType.Day;

    private GalControl galControl;
    private bool completed;

    private void Start()
    {
        if (StoryManager.Instance != null
            && StoryManager.Instance.currentData.chapter == 1
            && StoryManager.Instance.currentData.day == 0
            && StoryManager.Instance.currentData.scene == 1)
        {
            enabled = false;
            return;
        }

        galControl = GetComponentInChildren<GalControl>(true);
        if (galControl == null)
        {
            Debug.LogError("GalDayAdvanceOnComplete：子对象中没有 GalControl。", this);
            enabled = false;
            return;
        }

        galControl.PlaybackCompleted += OnPlaybackCompleted;
    }

    private void OnDestroy()
    {
        if (galControl != null)
            galControl.PlaybackCompleted -= OnPlaybackCompleted;
    }

    private void OnPlaybackCompleted()
    {
        AdvanceAndLoad();
    }

    // 其他脚本也可以调用它，复用相同的存档、销毁和切场景流程。
    public void AdvanceAndLoad()
    {
        if (completed)
            return;

        completed = true;

        if (StoryManager.Instance == null)
        {
            Debug.LogError("GalDayAdvanceOnComplete：不存在 StoryManager。", this);
            return;
        }

        switch (advanceType)
        {
            case AdvanceType.Scene:
                StoryManager.Instance.NextScene();
                break;
            case AdvanceType.Chapter:
                StoryManager.Instance.Nextchapter();
                break;
            default:
                StoryManager.Instance.Nextday();
                break;
        }

        StoryManager.Instance.SaveGame();

        Destroy(gameObject);
        SceneManager.LoadScene(targetScene);
    }
}