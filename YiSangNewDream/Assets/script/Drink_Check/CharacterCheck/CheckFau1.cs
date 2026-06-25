using UnityEngine;

public class CheckFau1 : MonoBehaviour
{
    private const int RequiredBitter = 2;
    private const int RequiredFresh = 2;

    [Header("当前饮品数值")]
    [SerializeField] private int bitter;
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
            Debug.LogError("CheckFau1 未在饮品对象上找到 finish_property_two", other);
            return;
        }

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("CheckFau1 缺少 Plot_Dy 组件", this);
            return;
        }

        bitter = drink.Bitter;
        fresh = drink.fresh;

        bool passed = bitter >= RequiredBitter && fresh >= RequiredFresh;
        plot.x = passed ? 1 : 2;
        plot.PlayNewPlot(passed ? successPath : failurePath);

        Debug.Log(passed
            ? $"浮士德判定成功：苦味 {bitter}，清新度 {fresh}"
            : $"浮士德判定失败：苦味 {bitter}，清新度 {fresh}");

        Destroy(other.gameObject);
    }
}
