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

    void Start()
    {
        // 按钮绑定事件
        if (redoButton != null)
            redoButton.onClick.AddListener(Redo);
        else
            Debug.LogWarning("未绑定重做按钮");
    }

    void OnMouseDown()
    {
        if (obj != null) return; // 如果已经有一个杯子，避免重复生成

        obj = Instantiate(_Prefeb, parent);//使用父物体约束
        obj.transform.localPosition = spawnPosition;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;


        // 动画初始为 0，再放大
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    void Redo()
    {
        if (obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }
}
//其实这里还有一个bug：如果杯子一多那就会乱生成