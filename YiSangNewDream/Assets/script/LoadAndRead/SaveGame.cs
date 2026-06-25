using UnityEngine;

public class SaveGame : MonoBehaviour
{
    private void Start()
    {
        if (StoryManager.Instance == null)
        {
            Debug.LogError("SaveGame：场景中不存在 StoryManager，无法保存游戏。", this);
            return;
        }

        StoryManager.Instance.SaveGame();
    }
}
