using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalReturn : MonoBehaviour
{
    [SerializeField]
    private string targetScene = "Before";

    public void Return()
    {
        SceneManager.LoadScene(targetScene);
    }
}
