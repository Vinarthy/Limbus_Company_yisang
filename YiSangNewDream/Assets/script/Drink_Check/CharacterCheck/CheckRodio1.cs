using UnityEngine;

public class CheckRodio1 : MonoBehaviour
{
    [Header("首次判定条件")]
    [SerializeField] private int minSweet = 1;
    [SerializeField] private int maxSweet = 3;
    [SerializeField] private int minSour = 2;
    [SerializeField] private int maxSour = 4;
    [SerializeField] private int minFresh = 1;

    [Header("剧情路径")]
    [SerializeField] private string successPath;
    [SerializeField] private string failurePath;
    [SerializeField] private string follow2Path;
    [SerializeField] private string follow3Path;
    [SerializeField] private string follow4Path;

    private int lastDrinkId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink") || other.gameObject.GetInstanceID() == lastDrinkId)
            return;

        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
            return;

        string nextPath = null;
        int nextPhase = plot.x;

        if (plot.x == 0)
        {
            finish_property_two drink = other.GetComponent<finish_property_two>();
            if (drink == null)
                return;

            bool passed = drink.Sweet >= minSweet && drink.Sweet <= maxSweet
                && drink.Sour >= minSour && drink.Sour <= maxSour
                && drink.fresh >= minFresh;
            nextPhase = passed ? 1 : 2;
            nextPath = passed ? successPath : failurePath;
        }
        else if (plot.x == 3)
        {
            nextPhase = 4;
            nextPath = follow2Path;
        }
        else if (plot.x == 4)
        {
            nextPhase = 5;
            nextPath = follow3Path;
        }
        else if (plot.x == 5)
        {
            nextPhase = 6;
            nextPath = follow4Path;
        }

        if (string.IsNullOrEmpty(nextPath))
            return;

        lastDrinkId = other.gameObject.GetInstanceID();
        plot.x = nextPhase;
        plot.PlayNewPlot(nextPath);
        Destroy(other.gameObject);
    }
}
