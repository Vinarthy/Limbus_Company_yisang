using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//事件发布器
public class RecordLineNumber : MonoBehaviour
{
    public int Num = 1;

    // 关键：事件
    public event Action<int> OnLineChanged;

    public void RecordLineNumberFunc(int num)
    {
        Num = num;

        // 通知所有监听者
        OnLineChanged?.Invoke(num);
    }
}

