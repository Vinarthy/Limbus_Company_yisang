using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test-scene-only sample bag items. Remove this file once the real JSON bag renderer is ready.
/// </summary>
public class BagpackTestItems : MonoBehaviour
{
    [Header("References")]
    public RectTransform content;

    [Header("Layout")]
    public int testItemCount = 12;
    public int columnCount = 4;
    public Vector2 itemSize = new Vector2(400f, 400f);
    public Vector2 spacing = new Vector2(210f, 160f);
    public Vector2 padding = new Vector2(60f, 40f);

    [Header("Visual")]
    public Sprite itemSprite;
    public Image.Type itemImageType = Image.Type.Simple;
    public Color itemColor = new Color(0.78f, 0.58f, 0.25f, 1f);

    private void Start()
    {
        if (content == null || content.childCount > 0)
            return;

        int safeColumnCount = Mathf.Max(1, columnCount);
        int rowCount = Mathf.CeilToInt(testItemCount / (float)safeColumnCount);
        float requiredHeight = padding.y * 2f
            + rowCount * itemSize.y
            + Mathf.Max(0, rowCount - 1) * spacing.y;
        content.sizeDelta = new Vector2(content.sizeDelta.x, requiredHeight);

        for (int index = 0; index < testItemCount; index++)
        {
            GameObject item = new GameObject(
                $"Test Bag Item {index + 1:00}",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image));

            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.SetParent(content, false);
            itemRect.anchorMin = new Vector2(0f, 1f);
            itemRect.anchorMax = new Vector2(0f, 1f);
            itemRect.pivot = new Vector2(0.5f, 0.5f);
            itemRect.sizeDelta = itemSize;

            int column = index % safeColumnCount;
            int row = index / safeColumnCount;
            itemRect.anchoredPosition = new Vector2(
                padding.x + itemSize.x * 0.5f + column * (itemSize.x + spacing.x),
                -(padding.y + itemSize.y * 0.5f + row * (itemSize.y + spacing.y)));

            Image image = item.GetComponent<Image>();
            image.sprite = itemSprite;
            image.type = itemImageType;
            image.color = itemColor;
        }
    }
}