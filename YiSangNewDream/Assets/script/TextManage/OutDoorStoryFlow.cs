using UnityEngine;
using UnityEngine.SceneManagement;

public class OutDoorStoryFlow : MonoBehaviour
{
    [Header("CG 剧情")]
    [SerializeField] private GameObject cgRoot;
    [SerializeField] private GalControl cgDialog;

    [Header("气泡剧情")]
    [SerializeField] private GameObject characterStage;
    [SerializeField] private Plot_Dy characterDialog;

    [Header("剧情结束")]
    [SerializeField] private string targetScene = "Before";

    private bool characterStarted;
    private bool completed;

    private void Start()
    {
        if (cgRoot == null || cgDialog == null || characterStage == null || characterDialog == null)
        {
            Debug.LogError("OutDoorStoryFlow：CG 或气泡剧情引用未配置完整。", this);
            enabled = false;
            return;
        }

        characterStage.SetActive(false);
        cgRoot.SetActive(true);
        cgDialog.PlaybackCompleted += StartCharacterDialog;
    }

    private void Update()
    {
        if (characterStarted && !completed && characterDialog.dialogFinished)
            AdvanceDay();
    }

    private void OnDestroy()
    {
        if (cgDialog != null)
            cgDialog.PlaybackCompleted -= StartCharacterDialog;
    }

    private void StartCharacterDialog()
    {
        if (characterStarted)
            return;

        characterStarted = true;
        cgDialog.PlaybackCompleted -= StartCharacterDialog;
        cgRoot.SetActive(false);
        characterStage.SetActive(true);
    }

    public void AdvanceDay()
    {
        if (completed)
            return;

        if (StoryManager.Instance == null)
        {
            Debug.LogError("OutDoorStoryFlow：不存在 StoryManager，无法推进天数。", this);
            enabled = false;
            return;
        }

        completed = true;
        StoryManager.Instance.Nextday();
        StoryManager.Instance.SaveGame();
        SceneManager.LoadScene(targetScene);
    }
}