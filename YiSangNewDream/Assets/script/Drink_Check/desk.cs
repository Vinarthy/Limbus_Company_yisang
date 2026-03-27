using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class desk : MonoBehaviour
{
    //任务系统，便签里面提示要做的茶的指标是什么样子的
    [Header("便签内容")]
    public string notename;//这个我决定改成任务内容，从json那里传进来得了
    public TextMeshProUGUI text;

    private void Start()
    {
        text.text = notename;
    }
}
