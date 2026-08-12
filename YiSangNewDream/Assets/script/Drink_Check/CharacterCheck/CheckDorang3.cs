using UnityEngine;

public class CheckDorang3 : MonoBehaviour
{
    private const string ResultPath = "Dialog/Chapter1/S5/3TF";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink")) return;
        Plot_Dy plot = GetComponent<Plot_Dy>();
        if (plot == null)
        {
            Debug.LogError("CheckDorang3 needs Plot_Dy.", this);
            return;
        }
        plot.x = 1;
        plot.PlayNewPlot(ResultPath);
        Destroy(other.gameObject);
    }
}