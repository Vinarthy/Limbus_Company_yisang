using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Taskshow : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    //这个脚本管按了顶栏然后显示任务的脚本
    //理念是从上面移动下来
    //显示：先true再从上面移下来
    //消失：先移上去再消失
    //任务内容传参得让别的脚本负责。
    //数值：主要改位置的Y，原来的Y是-4.2，需要缩进去的话y是3.75
    public GameObject TaskNote;
    private RectTransform rt;

    private float hideY = 3.75f;
    private float showY = -4.2f;

    void Start()
    {
        rt = TaskNote.GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        pos.y = hideY;
        rt.anchoredPosition = pos;

        TaskNote.SetActive(false);
    }
    void show()
    {
        // 先激活
        TaskNote.SetActive(true);
        rt.DOKill();
        Vector2 pos = rt.anchoredPosition;
        pos.y = hideY;
        rt.anchoredPosition = pos;

        rt.DOAnchorPosY(showY, 0.3f).SetEase(Ease.OutCubic);
    }

    void unshow()
    {
        // 防止动画叠加
        rt.DOKill();

        // 向上移动隐藏
        rt.DOAnchorPosY(hideY, 0.3f)
          .SetEase(Ease.OutCubic)
          .OnComplete(() =>
          {
              TaskNote.SetActive(false); // 动画结束再隐藏
          });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("展示任务");
        show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("关闭任务");
        unshow();
    }
}
