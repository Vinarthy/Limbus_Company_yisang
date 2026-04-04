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

    void Start()
    {
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

            case 5:
                Finish();
                break;
        }
    }

    void function()
    {
        Debug.Log("角色动作触发");
    }

    void Finish()
    {
        Debug.Log("结束逻辑");
    }
}
