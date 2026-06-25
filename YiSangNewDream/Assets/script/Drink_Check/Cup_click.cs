using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cup_click : MonoBehaviour
{
    //此脚本是重做和制作逻辑
    //通过按杯子生成对应的预制件，
    public GameObject _Prefeb;   // 生成的预制体
    private GameObject obj;      // 实例化制作后存储它
    public Button redoButton;    // 绑定 UI 按钮
    public Vector3 spawnPosition;//生成坐标
    public Transform parent;//父物体谢谢你

    // 其它杯子
    public Cup_click[] otherCups;

    // 警告符
    public GameObject warningObject;

    private bool canCreate = true;

    void Start()
    {
        if (redoButton != null)
            redoButton.onClick.AddListener(Redo);
    }

    void OnMouseDown()
    {
        if (!canCreate)
        {
            if (warningObject != null)
            {
                warningObject.SetActive(true);
            }
            return;
        }

        if (obj != null)
            return;

        obj = Instantiate(_Prefeb, parent);

        obj.transform.localPosition = spawnPosition;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.zero;

        obj.transform
            .DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack);

        // 禁用其它杯子
        foreach (Cup_click cup in otherCups)
        {
            if (cup != null)
            {
                cup.canCreate = false;
            }
        }
    }

    void Redo()
    {
        if (obj != null)
        {
            Destroy(obj);
            obj = null;
        }

        // 恢复其它杯子
        foreach (Cup_click cup in otherCups)
        {
            if (cup != null)
            {
                cup.canCreate = true;
            }
        }
    }
    public void ResetCupState()
    {
        canCreate = true;

        if (obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }
    //公共恢复
}
//其实这里还有一个bug：如果杯子一多那就会乱生成