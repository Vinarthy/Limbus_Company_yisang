using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl1 : MonoBehaviour
{
    //规划：1.写几个管理动作的函数，行数由RecordLineNumber脚本传进来
    //这里也放状态机
    //2.补一个该不该结束对话召唤制作UI的新功能
    //其实现在唯一的问题是怎么同步re.Num的消息。

    private RecordLineNumber re;
    public GameObject targetGameObject;
    private Plot_Dy DialogNum;

    void Start()
    {
        DialogNum = GetComponent<Plot_Dy>();
        re = GetComponent<RecordLineNumber>();
        // 订阅事件
        re.OnLineChanged += OnLineChanged;
    }

    void OnDestroy()
    {
        // 取消订阅（避免内存泄漏）
        if (re != null)
            re.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        int num = DialogNum.x;
        if (num == 0)
        {
            // 在这里写你的“按行触发逻辑”
            switch (line)
            {
                case 1:
                    Debug.Log("执行第1行逻辑");
                    break;

                case 3:
                    Debug.Log("执行第3行动作");
                    function();
                    break;

                case 7:
                    StartCoroutine(WaitAndEnable());
                    break;
            }
        }
        else if(num == 1)
        {
            switch (line)
            {
                case 1:
                    Debug.Log("这是判定成功了");
                    break;
            }
        }
        else if (num == 2)
        {
            switch (line)
            {
                case 1:
                    Debug.Log("这是判定失败了");
                    break;
            }
        }
    }

    void function()
    {
        Debug.Log("角色动作触发");
    }

    IEnumerator WaitAndEnable()
    {
        yield return new WaitForSeconds(0.3f);

        if (targetGameObject == null) yield break;

        targetGameObject.SetActive(true);

        CanvasGroup cg = targetGameObject.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = targetGameObject.AddComponent<CanvasGroup>();

        // 初始状态：完全透明 + 不可交互
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        // 渐显（1.5s）
        cg.DOFade(1f, 0.5f)
          .SetEase(Ease.Linear)
          .OnComplete(() =>
          {
              // 动画结束后恢复交互
              cg.interactable = true;
              cg.blocksRaycasts = true;
          });
    }
}
