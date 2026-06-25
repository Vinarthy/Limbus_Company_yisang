using System;
using UnityEngine;

public class CheckDongbai1 : MonoBehaviour
{
    private const string RequiredIngredient = "camellias";
    private const string MissingIngredientMessage = "未放\"山茶花\"";

    [SerializeField] private string resultPath;
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Drink"))
            return;

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("CheckDongbai1 缺少 Plot_Dy", this);
            return;
        }

        finish_property_two drink = other.GetComponent<finish_property_two>();
        if (drink == null)
        {
            Debug.LogError("CheckDongbai1 未在饮品对象上找到 finish_property_two", other);
            return;
        }

        if (!ContainsCamellias(drink.names))
        {
            plot.ShowNarrationNotice(MissingIngredientMessage, 1f);
            return;
        }

        triggered = true;
        plot.x = 1;
        plot.PlayNewPlot(resultPath);
        Destroy(other.gameObject);
    }

    private static bool ContainsCamellias(string[] ingredientNames)
    {
        if (ingredientNames == null)
            return false;

        foreach (string ingredientName in ingredientNames)
        {
            if (string.Equals(
                    ingredientName?.Trim(),
                    RequiredIngredient,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}