using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GallaryView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Image conclusionImage;

    private readonly Dictionary<int, GallarySlot> slots = new Dictionary<int, GallarySlot>();
    private static readonly Color32 LockedItemColor = new Color32(182, 182, 182, 255);
    private int selectedId;
    private bool subscribed;

    private sealed class GallarySlot
    {
        public Transform transform;
        public GameObject lockedImage;
        public Image slotImage;
        public Image itemImage;
        public Button button;
    }

    private void Awake()
    {
        content = content != null ? content : FindChild(transform, "Content");
        Transform conclusion = FindChild(transform, "Conclusion");

        TMP_Text[] texts = conclusion != null ? conclusion.GetComponentsInChildren<TMP_Text>(true) : null;
        if (texts != null)
        {
            if (itemNameText == null && texts.Length > 0)
                itemNameText = texts[0];
            if (itemDescriptionText == null && texts.Length > 1)
                itemDescriptionText = texts[1];
        }

        CacheSlots();

        if (conclusionImage != null)
            conclusionImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Subscribe();
        Refresh();
    }

    private void Start()
    {
        Subscribe();
        Refresh();
    }

    private void OnDisable()
    {
        if (subscribed && GallaryManager.Instance != null)
            GallaryManager.Instance.GalleryChanged -= Refresh;

        subscribed = false;
    }

    public void Refresh()
    {
        if (GallaryManager.Instance == null || GallaryManager.Instance.galleryList == null)
            return;

        foreach (KeyValuePair<int, GallarySlot> pair in slots)
        {
            GallaryManager.GallaryItem item = GallaryManager.Instance.GetGallaryItem(pair.Key);
            pair.Value.transform.gameObject.SetActive(item != null);
            if (item == null)
                continue;

            pair.Value.lockedImage.SetActive(!item.unlocked);
            pair.Value.slotImage.color = item.unlocked ? Color.white : LockedItemColor;
            pair.Value.itemImage.enabled = item.unlocked;
            pair.Value.button.interactable = item.unlocked;
        }

        if (GallaryManager.Instance.galleryList.Count == 0)
            return;

        GallaryManager.GallaryItem selectedItem = GallaryManager.Instance.GetGallaryItem(selectedId);
        if (selectedItem == null || !selectedItem.unlocked)
            selectedItem = GallaryManager.Instance.galleryList.Find(item => item.unlocked);

        if (selectedItem == null)
        {
            if (conclusionImage != null)
                conclusionImage.gameObject.SetActive(false);

            return;
        }

        selectedId = selectedItem.id;
        ShowItem(selectedItem);
    }

    private void CacheSlots()
    {
        if (content == null)
        {
            Debug.LogError("GallaryView requires the Content transform.", this);
            return;
        }

        for (int i = 1; ; i++)
        {
            Transform slotTransform = content.Find("Shop Item Slot " + i);
            if (slotTransform == null)
                break;

            Transform lockedImageTransform = slotTransform.Find("Image");
            Transform itemImageTransform = slotTransform.Find("Image (1)");
            Image slotImage = slotTransform.GetComponent<Image>();
            Image itemImage = itemImageTransform != null ? itemImageTransform.GetComponent<Image>() : null;
            if (lockedImageTransform == null || slotImage == null || itemImage == null)
            {
                Debug.LogError("Gallary slot is missing Image or Image (1): " + slotTransform.name, this);
                continue;
            }

            Button button = slotTransform.GetComponent<Button>();
            if (button == null)
                button = slotTransform.gameObject.AddComponent<Button>();

            int slotId = i;
            button.onClick.AddListener(() => SelectItem(slotId));
            slots.Add(slotId, new GallarySlot
            {
                transform = slotTransform,
                lockedImage = lockedImageTransform.gameObject,
                slotImage = slotImage,
                itemImage = itemImage,
                button = button
            });
        }
    }

    private void SelectItem(int id)
    {
        selectedId = id;
        Refresh();
    }

    private void ShowItem(GallaryManager.GallaryItem item)
    {
        if (itemNameText != null)
            itemNameText.text = item.name;
        if (itemDescriptionText != null)
            itemDescriptionText.text = item.description;

        if (conclusionImage == null)
            return;

        Sprite sprite = item.unlocked ? LoadSprite(item.path) : null;
        conclusionImage.gameObject.SetActive(item.unlocked);
        conclusionImage.enabled = item.unlocked;
        if (sprite == null)
            return;

        conclusionImage.sprite = sprite;
    }

    private void Subscribe()
    {
        if (subscribed || GallaryManager.Instance == null)
            return;

        GallaryManager.Instance.GalleryChanged += Refresh;
        subscribed = true;
    }

    private static Transform FindChild(Transform root, string childName)
    {
        if (root == null)
            return null;

        foreach (Transform child in root)
        {
            if (child.name == childName)
                return child;

            Transform result = FindChild(child, childName);
            if (result != null)
                return result;
        }

        return null;
    }

    private static Sprite LoadSprite(string resourcePath)
    {
        return string.IsNullOrEmpty(resourcePath) ? null : Resources.Load<Sprite>(resourcePath);
    }
}
