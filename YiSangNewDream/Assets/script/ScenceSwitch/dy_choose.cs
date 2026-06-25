using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dy_choose : MonoBehaviour
{
    public GameObject TargetPrefeb;

    void OnMouseDown()
    {
        Debug.Log("鼠标按下了思密达");

        click c = click.Instance;

        if (c == null)
        {
            Debug.LogWarning("场景中没有找到click单例!");
            return;
        }

        if (TargetPrefeb == null)
        {
            Debug.LogWarning("TargetPrefeb为空!");
            return;
        }

        SceneManager.LoadScene("Middle");
    }
}