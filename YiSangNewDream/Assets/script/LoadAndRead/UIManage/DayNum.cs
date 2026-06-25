using TMPro;
using UnityEngine;

public class DayNum : MonoBehaviour
{
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
            tmpText.text = "Day0";
            return;
        }

        int day =
            StoryManager.Instance.currentData.day;

        tmpText.text = $"Day{day}";
    }
}