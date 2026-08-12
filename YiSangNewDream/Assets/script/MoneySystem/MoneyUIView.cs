using TMPro;
using UnityEngine;

public class MoneyUIView : MonoBehaviour
{
    private TextMeshProUGUI moneyText;


    void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }


    void Start()
    {
        //¼àÌý½ð±Ò±ä»¯
        MoneyManage.Instance.OnMoneyChanged += UpdateMoneyUI;
    }


    private void UpdateMoneyUI(int money)
    {
        moneyText.text = money.ToString();
    }


    private void OnDestroy()
    {
        if (MoneyManage.Instance != null)
        {
            MoneyManage.Instance.OnMoneyChanged -= UpdateMoneyUI;
        }
    }
}
