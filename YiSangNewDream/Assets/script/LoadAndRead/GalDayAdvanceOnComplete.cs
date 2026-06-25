using UnityEngine;
using UnityEngine.SceneManagement;

public class GalDayAdvanceOnComplete : MonoBehaviour
{
    [SerializeField] private string targetScene = "Before";

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
        if (completed)
            return;

        completed = true;

        if (StoryManager.Instance == null)
        {
            Debug.LogError("GalDayAdvanceOnComplete：不存在 StoryManager。", this);
            return;
        }

        StoryManager.Instance.Nextday();
        StoryManager.Instance.SaveGame();

        Destroy(gameObject);
        SceneManager.LoadScene(targetScene);
    }
}
