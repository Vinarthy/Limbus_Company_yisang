using UnityEngine;

public class CheckHong1 : MonoBehaviour
{
    [Header("判定条件（不包含边界）")]
    [SerializeField] private int maxSweetExclusive = 2;
    [SerializeField] private int minBitterExclusive = 1;
    [SerializeField] private int minFreshExclusive = 1;

    [Header("剧情路径")]
    [SerializeField] private string successPath;
    [SerializeField] private string failurePath;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Drink"))
            return;

        finish_property_two drink = other.GetComponent<finish_property_two>();
        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (drink == null || plot == null)
        {
            Debug.LogError("CheckHong1 缺少 finish_property_two 或 Plot_Dy", this);
            return;
        }

        triggered = true;
        bool passed = drink.Sweet < maxSweetExclusive
            && drink.Bitter > minBitterExclusive
            && drink.fresh > minFreshExclusive;

        plot.x = passed ? 1 : 2;
        plot.PlayNewPlot(passed ? successPath : failurePath);
        Destroy(other.gameObject);
    }
}
