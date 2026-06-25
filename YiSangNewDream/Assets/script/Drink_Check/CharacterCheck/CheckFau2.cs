using System;
using UnityEngine;

public class CheckFau2 : MonoBehaviour
{
    private static readonly string[] RequiredIngredients =
    {
        "hajia",
        "hajiaegg",
        "Pomelo"
    };

    private const string MissingIngredientMessage =
        "你没有一比一地复刻伞夫召唤仪式";

    [Header("饮品满足仪式要求后进入的剧情")]
    [SerializeField] private string resultPath;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Drink"))
            return;

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("CheckFau2 缺少 Plot_Dy 组件", this);
            return;
        }

        finish_property_two drink = other.GetComponent<finish_property_two>();
        if (drink == null)
        {
            Debug.LogError("CheckFau2 未在饮品对象上找到 finish_property_two", other);
            return;
        }

        if (!ContainsAllRequiredIngredients(drink.names))
        {
            plot.ShowNarrationNotice(MissingIngredientMessage, 1f);
            Debug.Log("Faust2 仪式判定失败：饮品缺少 hajia、hajiaegg 或 Pomelo。", other);
            return;
        }

        triggered = true;
        plot.x = 1;
        plot.PlayNewPlot(resultPath);
        Destroy(other.gameObject);
    }

    private static bool ContainsAllRequiredIngredients(string[] ingredientNames)
    {
        if (ingredientNames == null)
            return false;

        foreach (string requiredIngredient in RequiredIngredients)
        {
            bool found = false;

            foreach (string ingredientName in ingredientNames)
            {
                if (!string.Equals(
                        ingredientName?.Trim(),
                        requiredIngredient,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                found = true;
                break;
            }

            if (!found)
                return false;
        }

        return true;
    }
}