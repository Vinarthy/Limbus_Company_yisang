using UnityEngine;
using UnityEngine.SceneManagement;

public class Choose : MonoBehaviour
{
    [Header("要切换到的场景名")]
    public string targetSceneName;

    [Header("Resources 里的预制件路径，不要写 .prefab")]
    public string prefabPath;

    [Header("生成到哪个父物体下面")]
    public string parentName;

    private void OnMouseDown()
    {
        if (OneToAny.Instance == null)
        {
            Debug.LogError("场景中没有 OneToAny 单例对象");
            return;
        }

        OneToAny.Instance.SetTarget(
            targetSceneName,
            prefabPath,
            parentName
        );

        SceneManager.LoadScene(targetSceneName);
    }
}