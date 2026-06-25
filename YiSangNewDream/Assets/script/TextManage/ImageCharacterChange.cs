using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FaceSpriteData
{
    [Header("表情名，比如 Normal / Happy / Angry")]
    public string faceName;

    [Header("对应 Sprite")]
    public Sprite sprite;
}
public class ImageCharacterChange : MonoBehaviour
{
    [Header("角色表情 Image")]
    [SerializeField] private Image faceImage;

    [Header("默认表情名")]
    [SerializeField] private string defaultFace = "Normal";

    [Header("表情列表")]
    public List<FaceSpriteData> faceList = new List<FaceSpriteData>();

    private Dictionary<string, Sprite> faceMap = new Dictionary<string, Sprite>();

    private string currentFace = "";

    private void Awake()
    {
        if (faceImage == null)
            faceImage = GetComponent<Image>();

        InitFaceMap();

        ChangeFace(defaultFace);
    }

    private void InitFaceMap()
    {
        faceMap.Clear();

        foreach (FaceSpriteData data in faceList)
        {
            if (data == null)
                continue;

            if (string.IsNullOrEmpty(data.faceName))
                continue;

            if (data.sprite == null)
                continue;

            if (!faceMap.ContainsKey(data.faceName))
            {
                faceMap.Add(data.faceName, data.sprite);
            }
            else
            {
                Debug.LogWarning("表情名重复：" + data.faceName, this);
            }
        }
    }

    /// <summary>
    /// 外部接口：通过表情名切换
    /// </summary>
    public void ChangeFace(string faceName)
    {
        if (string.IsNullOrEmpty(faceName))
            return;

        if (currentFace == faceName)
            return;

        if (!faceMap.TryGetValue(faceName, out Sprite targetSprite))
        {
            Debug.LogWarning("没有找到表情：" + faceName, this);
            return;
        }

        currentFace = faceName;
        faceImage.sprite = targetSprite;
    }

    /// <summary>
    /// 外部接口：获取当前表情名
    /// </summary>
    public string GetCurrentFace()
    {
        return currentFace;
    }
}
