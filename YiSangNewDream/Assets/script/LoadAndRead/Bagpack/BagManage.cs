using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BagManage : MonoBehaviour
{
    [Header("Bag Grid")]
    public RectTransform content;
    public GameObject itemPrefab;
    public string itemIconChildName = "Icon";
    public int columnCount = 3;
    public Vector2 itemSize = new Vector2(400f, 400f);
    public Vector2 spacing = new Vector2(80f, 80f);
    public Vector2 padding = new Vector2(80f, 40f);

    [Header("Selected Item")]
    public Image selectedImage;
    public TMP_Text selectedNameText;
    public TMP_Text selectedDescriptionText;
    public Button button;

    private readonly List<GameObject> itemViews = new List<GameObject>();
    private BagReadAndLoad bagReadAndLoad;
    private DecorateRAndL decorateRAndL;
    private BagReadAndLoad.BagItem selectedItem;
    private TMP_Text buttonText;

    private void Awake()
    {
        bagReadAndLoad = new BagReadAndLoad();
        decorateRAndL = new DecorateRAndL();
        if (button != null)
        {
            buttonText = button.GetComponentInChildren<TMP_Text>(true);
            button.onClick.AddListener(ToggleSelectedItem);
        }
    }

    private void OnEnable()
    {
        SubscribeToShop();
        Reload();
    }

    private void Start()
    {
        SubscribeToShop();
    }

    private void OnDisable()
    {
        if (ShopManager.Instance != null)
            ShopManager.Instance.ShopChanged -= Reload;
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(ToggleSelectedItem);
    }

    public void Reload()
    {
        if (bagReadAndLoad == null)
            bagReadAndLoad = new BagReadAndLoad();
        if (content == null || itemPrefab == null)
        {
            Debug.LogWarning("BagManage needs Content and Item Prefab assigned.", this);
            return;
        }

        ClearItemViews();
        List<BagReadAndLoad.BagItem> bagItems = bagReadAndLoad.LoadBag();
        ConfigureContentHeight(bagItems.Count);

        for (int index = 0; index < bagItems.Count; index++)
        {
            GameObject itemView = Instantiate(itemPrefab, content);
            ConfigureItemPosition(itemView.transform as RectTransform, index);
            ConfigureItemVisual(itemView, bagItems[index]);
            ConfigureItemButton(itemView, bagItems[index]);
            itemViews.Add(itemView);
        }

        if (bagItems.Count > 0)
            SelectItem(bagItems[0]);
        else
        {
            selectedItem = null;
            UpdateActionButton();
        }
    }

    public void SelectItem(BagReadAndLoad.BagItem item)
    {
        selectedItem = item;
        if (selectedNameText != null)
            selectedNameText.text = item.name;
        if (selectedDescriptionText != null)
            selectedDescriptionText.text = item.description;
        if (selectedImage != null)
        {
            selectedImage.sprite = LoadSprite(item.path);
            selectedImage.gameObject.SetActive(selectedImage.sprite != null);
        }

        UpdateActionButton();
    }

    public Sprite LoadSprite(string resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
            return null;

        string path = resourcePath.Replace("Assets/Resources/", string.Empty).Replace(".png", string.Empty);
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null && !path.Contains("/"))
            sprite = Resources.Load<Sprite>("UI/BagPack/" + path);
        return sprite;
    }

    private void UpdateActionButton()
    {
        string resourcePath = null;
        bool canUseSelected = selectedItem != null
            && selectedItem.Type == "Music"
            && !string.IsNullOrWhiteSpace(selectedItem.name);

        if (selectedItem != null && selectedItem.Type == "Decorate")
            canUseSelected = DecorateConfig.TryGetResourcePath(selectedItem.name, out resourcePath);

        if (button != null)
            button.gameObject.SetActive(canUseSelected);
        if (!canUseSelected)
            return;

        bool isUsingSelected;
        if (selectedItem.Type == "Music")
        {
            isUsingSelected = SceneSwitchBGM.Instance != null
                && SceneSwitchBGM.Instance.mainBgmId == selectedItem.name;
        }
        else
        {
            DecorateManage.DecorateItem current = decorateRAndL.GetCurrentDecorate();
            isUsingSelected = current.name == selectedItem.name && current.path == resourcePath;
        }

        if (buttonText != null)
            buttonText.text = isUsingSelected ? "取消" : "使用";
    }

    private void ToggleSelectedItem()
    {
        if (selectedItem == null)
            return;

        if (selectedItem.Type == "Music")
        {
            ToggleSelectedMusic();
            return;
        }

        if (selectedItem.Type != "Decorate")
            return;

        if (!DecorateConfig.TryGetResourcePath(selectedItem.name, out string resourcePath))
            return;

        DecorateManage.DecorateItem current = decorateRAndL.GetCurrentDecorate();
        bool isUsingSelected = current.name == selectedItem.name && current.path == resourcePath;
        bool saved = isUsingSelected
            ? decorateRAndL.SaveDecorate("Default", "000")
            : decorateRAndL.SaveDecorate(selectedItem.name, resourcePath);
        if (!saved)
        {
            Debug.LogError("Failed to save Decorate selection.");
            return;
        }

        RefreshMiddleDecorate();
        UpdateActionButton();
    }

    private void ToggleSelectedMusic()
    {
        if (SceneSwitchBGM.Instance == null)
        {
            Debug.LogError("BagManage：不存在 SceneSwitchBGM，无法切换主 BGM。", this);
            return;
        }

        bool isUsingSelected = SceneSwitchBGM.Instance.mainBgmId == selectedItem.name;
        string targetBgmId = isUsingSelected
            ? SceneSwitchBGM.DefaultMainBgmId
            : selectedItem.name;

        SceneSwitchBGM.Instance.SetMainBGM(targetBgmId);
        UpdateActionButton();
    }

    private void RefreshMiddleDecorate()
    {
        if (SceneManager.GetActiveScene().name != "Middle")
            return;

        DecorateManage decorateManage = FindObjectOfType<DecorateManage>(true);
        if (decorateManage != null)
            decorateManage.Reload();
    }

    private void ConfigureItemVisual(GameObject itemView, BagReadAndLoad.BagItem item)
    {
        Image icon = FindItemIcon(itemView);
        if (icon == null)
            return;

        icon.sprite = LoadSprite(item.path);
        icon.enabled = icon.sprite != null;
    }

    private void ConfigureItemButton(GameObject itemView, BagReadAndLoad.BagItem item)
    {
        Button itemButton = itemView.GetComponent<Button>();
        if (itemButton != null)
            itemButton.onClick.AddListener(() => SelectItem(item));
    }

    private Image FindItemIcon(GameObject itemView)
    {
        if (!string.IsNullOrWhiteSpace(itemIconChildName))
        {
            Transform iconTransform = itemView.transform.Find(itemIconChildName);
            if (iconTransform != null && iconTransform.TryGetComponent(out Image icon))
                return icon;
        }

        return itemView.GetComponent<Image>();
    }

    private void SubscribeToShop()
    {
        if (ShopManager.Instance == null)
            return;

        ShopManager.Instance.ShopChanged -= Reload;
        ShopManager.Instance.ShopChanged += Reload;
    }

    private void ClearItemViews()
    {
        foreach (GameObject itemView in itemViews)
        {
            if (itemView != null)
                Destroy(itemView);
        }
        itemViews.Clear();
    }

    private void ConfigureContentHeight(int itemCount)
    {
        int safeColumnCount = Mathf.Max(1, columnCount);
        int rowCount = Mathf.CeilToInt(itemCount / (float)safeColumnCount);
        float height = padding.y * 2f + rowCount * itemSize.y + Mathf.Max(0, rowCount - 1) * spacing.y;
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }

    private void ConfigureItemPosition(RectTransform itemRect, int index)
    {
        if (itemRect == null)
            return;

        int safeColumnCount = Mathf.Max(1, columnCount);
        int column = index % safeColumnCount;
        int row = index / safeColumnCount;
        itemRect.anchorMin = new Vector2(0f, 1f);
        itemRect.anchorMax = new Vector2(0f, 1f);
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        itemRect.sizeDelta = itemSize;
        itemRect.anchoredPosition = new Vector2(
            padding.x + itemSize.x * 0.5f + column * (itemSize.x + spacing.x),
            -(padding.y + itemSize.y * 0.5f + row * (itemSize.y + spacing.y)));
    }
}