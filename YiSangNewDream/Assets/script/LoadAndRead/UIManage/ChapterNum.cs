using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterNum : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_Text tmpText;

    private void Start()
    {
        tmpText = GetComponent<TMP_Text>();

        RefreshText();
    }

    public void RefreshText()
    {
        if (StoryManager.Instance == null)
        {
            tmpText.text = "ตฺ0ีย";
            return;
        }

        int chapter =
            StoryManager.Instance.currentData.chapter;

        tmpText.text = $"ตฺ{chapter}ีย";
    }
}
