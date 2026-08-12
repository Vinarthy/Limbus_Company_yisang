using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//˼·��ѡ�н�ɫ֮�󣬺����white��ɿɼ�״̬������뿪֮���ɲ��ɼ�
//����״̬��֮��ͼӼ��д�������飬����ôд
public class Choice : MonoBehaviour
{

    public GameObject Black;
    private bool blackWasActive;
    // Start is called before the first frame update
    void Start()
    {
        if (Black != null)
        {
            blackWasActive = Black.activeSelf;
        }
    }

    void OnMouseEnter()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // ���������ʱ����ʾBlack����
        if (Black != null)
        {
            Black.SetActive(true);
        }
    }
    void OnMouseExit()
    {
        // ������Ƴ�ʱ������Black����
        if (Black != null)
        {
            Black.SetActive(false);
        }
    }
}
