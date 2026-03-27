using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//这个脚本写的是鼠标进去就有信息出来，主要是显示属性和一些介绍
public class Tip : MonoBehaviour
{
    private DragAndReturn2D drag;
    private bool isHovering;
    public GameObject tip;
    [SerializeField]private string Message;
    public GameObject canvas_;

    private GameObject tipInstance;//当前实例
    void Awake()
    {
        drag = GetComponent<DragAndReturn2D>();
    }
    void Update()
    {
        if (drag != null && drag.isDragging)
        {
            ForceHide();
            return;
        }
        Vector2 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(
            mouseWorldPos,
            Vector2.zero,
            Mathf.Infinity,
            1 << gameObject.layer
        );

        bool nowHover =
            hit.collider != null &&
            hit.collider.gameObject == gameObject;

        if (nowHover && !isHovering)
        {
            isHovering = true;
            ShowInfo();
        }
        else if (!nowHover && isHovering)
        {
            isHovering = false;
            HideInfo();
        }
    }

    void ForceHide()
    {
        if (isHovering)
        {
            isHovering = false;
            HideInfo();
        }
    }

    void ShowInfo()
    {
        if (tipInstance != null) return;

        tipInstance = Instantiate(tip,canvas_.transform);

        // 如果是 UI，一般要挂在 Canvas 下
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            tipInstance.transform.SetParent(canvas.transform, false);
        }

        // === 写入 Message（子物体的子物体）===
        Transform textTrans = tipInstance.transform.GetChild(0).GetChild(0);

        TMP_Text tmp = textTrans.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = Message;
        }
    }

    void HideInfo()
    {
        if (tipInstance != null)
        {
            Destroy(tipInstance);
            tipInstance = null;
        }
    }
}
