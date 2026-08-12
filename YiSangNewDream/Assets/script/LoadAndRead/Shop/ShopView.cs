using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Button buyButton;

    private readonly List<ShopSlot> slots = new List<ShopSlot>();
    private static readonly Color32 PurchasedItemColor = new Color32(177, 177, 177, 255);
    private int selectedIndex;
    private bool subscribed;

    private sealed class ShopSlot
    {
        public Transform transform;
        public GameObject purchasedImage;
        public Image itemImage;
        public Button button;
    }

    private void Awake()
    {
        if (content == null)
            content = transform.Find("Content");

        CacheSlots();

        if (buyButton != null)
            buyButton.onClick.AddListener(BuySelectedItem);
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
        if (subscribed && ShopManager.Instance != null)
            ShopManager.Instance.ShopChanged -= Refresh;

        subscribed = false;
    }

    public void Refresh()
    {
        if (ShopManager.Instance == null || ShopManager.Instance.shopList == null)
            return;

        for (int i = 0; i < slots.Count; i++)
        {
            bool hasItem = i < ShopManager.Instance.shopList.Count;
            slots[i].transform.gameObject.SetActive(hasItem);

            if (hasItem)
            {
                bool purchased = ShopManager.Instance.shopList[i].purchased;
                slots[i].purchasedImage.SetActive(purchased);
                slots[i].itemImage.color = purchased ? PurchasedItemColor : Color.white;
            }
        }

        if (ShopManager.Instance.shopList.Count == 0)
            return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, ShopManager.Instance.shopList.Count - 1);
        ShowItem(ShopManager.Instance.shopList[selectedIndex]);
    }

    private void CacheSlots()
    {
        if (content == null)
        {
            Debug.LogError("ShopView requires the Content transform.", this);
            return;
        }

        for (int i = 1; ; i++)
        {
            Transform slotTransform = content.Find("Shop Item Slot " + i);
            if (slotTransform == null)
                break;

            Transform purchasedImageTransform = slotTransform.Find("Image");
            Transform itemImageTransform = slotTransform.Find("Image (1)");
            Image itemImage = itemImageTransform != null ? itemImageTransform.GetComponent<Image>() : null;
            if (purchasedImageTransform == null || itemImage == null)
            {
                Debug.LogError("Shop item slot is missing Image or Image (1): " + slotTransform.name, this);
                continue;
            }

            Button button = slotTransform.GetComponent<Button>();
            if (button == null)
                button = slotTransform.gameObject.AddComponent<Button>();

            ShopSlot slot = new ShopSlot
            {
                transform = slotTransform,
                purchasedImage = purchasedImageTransform.gameObject,
                itemImage = itemImage,
                button = button
            };

            int slotIndex = slots.Count;
            button.onClick.AddListener(() => SelectItem(slotIndex));
            slots.Add(slot);
        }
    }

    private void SelectItem(int index)
    {
        selectedIndex = index;
        Refresh();
    }

    private void ShowItem(ShopManager.ShopItem item)
    {
        itemNameText.text = item.name;
        itemDescriptionText.text = item.description;
        buyButton.gameObject.SetActive(!item.purchased);
    }

    private void BuySelectedItem()
    {
        if (ShopManager.Instance == null || ShopManager.Instance.shopList == null)
            return;

        ShopManager.Instance.BuyShopItem(ShopManager.Instance.shopList[selectedIndex].id);
    }

    private void Subscribe()
    {
        if (subscribed || ShopManager.Instance == null)
            return;

        ShopManager.Instance.ShopChanged += Refresh;
        subscribed = true;
    }
}