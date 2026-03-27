using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Ps.这个脚本挂载到
public class DialogUI : MonoBehaviour
{

    public TextMeshProUGUI textField;//这个的目标是一个预制件的，有点难办了，子物体
    public Typewriter typewriter;  // 引用你那个打字机脚本


    public void Setup(string text)
    {
        typewriter.StartTyping(text);
    }

    public bool IsTyping()
    {
        return typewriter.IsTyping();
    }
    public void CompleteLine()
    {
        typewriter.CompleteLine();
    }
}
