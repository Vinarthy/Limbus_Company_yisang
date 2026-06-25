using UnityEngine;
using UnityEngine.SceneManagement;

public class CleanAndReturn : MonoBehaviour
{
    [SerializeField]
    private string targetScene = "Before";

    public void Return()
    {
        Debug.Log("Return被调用");
        if (click.Instance != null)
        {
            click.Instance.ClearSpawnedCharacters();
        }

        switch (TimeBool.Instance.AdvanceType)
        {
            case StoryAdvanceType.Scene:
                StoryManager.Instance.NextScene();
                break;

            case StoryAdvanceType.Day:
                StoryManager.Instance.Nextday();
                break;

            case StoryAdvanceType.Chapter:
                StoryManager.Instance.Nextchapter();
                break;
        }

        TimeBool.Instance.AdvanceType =
            StoryAdvanceType.None;

        StoryManager.Instance.SaveGame();

        SceneManager.LoadScene(targetScene);
    }
}
//1.判断timebool的状态，通过状态来判定调用什么函数