using UnityEngine;

public class Checksh1 : MonoBehaviour
{
    [Header("饮品完成后直接进入的剧情")]
    [SerializeField] private string resultPath;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Drink"))
            return;

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("Checksh1 缺少 Plot_Dy 组件", this);
            return;
        }

        triggered = true;
        plot.x = 1;
        plot.PlayNewPlot(resultPath);
        Destroy(other.gameObject);
    }
}
