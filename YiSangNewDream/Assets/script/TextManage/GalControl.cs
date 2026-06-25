using System;
using TMPro;
using UnityEngine;

public class GalControl : MonoBehaviour
{
    [Header("Resources 里的 JSON 路径，不要写 .json")]
    public string dialogPath = "Dialog/gal1";

    [Header("角色名文本")]
    public TextMeshProUGUI nameText;

    [Header("打字机")]
    public Typewriter typewriter;

    [Header("每行剧情事件控制")]
    public GalBase galEvent;

    private PlayerInfoList playerInfoList;
    private int currentIndex;
    private bool playbackCompleted;

    public event Action PlaybackCompleted;

    private void Start()
    {
        LoadDialog();
        PlayCurrentLine();
    }

    private void Update()
    {
        if (playbackCompleted)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            Next();
    }

    private void LoadDialog()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dialogPath);
        if (textAsset == null)
        {
            Debug.LogError("没有找到 JSON 文件：" + dialogPath, this);
            return;
        }

        playerInfoList = JsonUtility.FromJson<PlayerInfoList>(textAsset.text);
        if (playerInfoList == null || playerInfoList.dialogList == null || playerInfoList.dialogList.Count == 0)
            Debug.LogError("JSON 解析失败或 dialogList 为空：" + dialogPath, this);
    }

    private void PlayCurrentLine()
    {
        if (playerInfoList == null || playerInfoList.dialogList == null)
            return;

        if (currentIndex >= playerInfoList.dialogList.Count)
        {
            if (!playbackCompleted)
            {
                playbackCompleted = true;
                PlaybackCompleted?.Invoke();
            }

            Debug.Log("剧情播放结束");
            return;
        }

        PlayerInfo info = playerInfoList.dialogList[currentIndex];

        if (nameText != null)
            nameText.text = info.Name == "default" ? "" : info.Name;

        if (typewriter != null)
            typewriter.StartTyping(info.Speak);

        if (galEvent != null)
            galEvent.OnLine(info.Num, info);
    }

    public void Next()
    {
        if (playbackCompleted)
            return;

        if (typewriter != null && typewriter.IsTyping())
        {
            typewriter.CompleteLine();
            return;
        }

        currentIndex++;
        PlayCurrentLine();
    }
}
