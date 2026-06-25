using UnityEngine;

public class CheckDon2 : MonoBehaviour
{
    private const int RequiredSweet = 3;
    private const int RequiredFresh = 2;

    [Header("当前饮品数值")]
    [SerializeField] private int sweet;
    [SerializeField] private int fresh;

    [Header("判定后的剧情路径")]
    [SerializeField] private string successPath;
    [SerializeField] private string failurePath;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink"))
            return;

        finish_property_two drink = other.GetComponent<finish_property_two>();
        if (drink == null)
        {
            Debug.LogError("CheckDon2 未在饮品对象上找到 finish_property_two", other);
            return;
        }

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("CheckDon2 缺少 Plot_Dy 组件", this);
            return;
        }

        sweet = drink.Sweet;
        fresh = drink.fresh;

        bool passed = sweet > RequiredSweet && fresh > RequiredFresh;
        plot.x = passed ? 1 : 2;
        plot.PlayNewPlot(passed ? successPath : failurePath);

        Debug.Log(passed
            ? $"堂吉诃德第二段判定成功：甜度 {sweet}，清新度 {fresh}"
            : $"堂吉诃德第二段判定失败：甜度 {sweet}，清新度 {fresh}");

        Destroy(other.gameObject);
    }
}
