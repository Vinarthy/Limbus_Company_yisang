using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI textdisplay1;
    public float waitingforsecond = 0.05f;
    private Coroutine typingcoroutine;//协程变量
    private bool isTyping;//判断是否在打字



    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        textdisplay1.text = text;
        textdisplay1.maxVisibleCharacters = 0;
        for (int i = 0; i <= text.Length; i++)
        {
            textdisplay1.maxVisibleCharacters = i;
            yield return new WaitForSeconds(waitingforsecond);
        }
        isTyping=false;
    }
    public void StartTyping(string text)
    {
        if (typingcoroutine != null)
        {
            StopCoroutine(typingcoroutine);
        }
        typingcoroutine = StartCoroutine(TypeLine(text));
    }
    public void CompleteLine()
    {
        if(typingcoroutine != null)
        {
            StopCoroutine(typingcoroutine);
        }
        textdisplay1.maxVisibleCharacters=textdisplay1.text.Length;//捏？
        isTyping = false;
    }
    public bool IsTyping()
    {
        return isTyping;
    }
}
