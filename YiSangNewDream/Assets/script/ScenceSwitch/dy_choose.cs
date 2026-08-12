using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class dy_choose : MonoBehaviour
{
    public GameObject TargetPrefeb;

    void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Debug.Log("��갴����˼�ܴ�");

        click c = click.Instance;

        if (c == null)
        {
            Debug.LogWarning("������û���ҵ�click����!");
            return;
        }

        if (TargetPrefeb == null)
        {
            Debug.LogWarning("TargetPrefebΪ��!");
            return;
        }

        SceneManager.LoadScene("Middle");
    }
}