using UnityEngine;

public class CheckDon5 : MonoBehaviour
{
    private const int RequiredSweet = 3;
    private const int RequiredFresh = 2;
    [SerializeField] private string successPath;
    [SerializeField] private string failurePath;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink")) return;
        finish_property_two drink = other.GetComponent<finish_property_two>();
        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (drink == null || plot == null)
        {
            Debug.LogError("CheckDon5 needs finish_property_two and Plot_Dy.", this);
            return;
        }
        bool passed = drink.Sweet > RequiredSweet && drink.fresh > RequiredFresh;
        plot.x = passed ? 1 : 2;
        plot.PlayNewPlot(passed ? successPath : failurePath);
        Destroy(other.gameObject);
    }
}