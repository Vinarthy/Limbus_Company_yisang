using UnityEngine;

public class GarbageBin : MonoBehaviour
{
    [Header("销毁饮料后激活的物体")]
    public GameObject targetObject;

    [Header("垃圾桶盖动画")]
    [SerializeField] private GarbageBinAnimation garbageBinAnimation;

    private GameObject drinkInBin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink"))
            return;

        if (garbageBinAnimation == null)
            garbageBinAnimation = GetComponent<GarbageBinAnimation>();

        drinkInBin = other.gameObject;
        garbageBinAnimation?.Open();

        if (garbageBinAnimation != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("garbage");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != drinkInBin)
            return;

        drinkInBin = null;
        garbageBinAnimation?.Close();
    }

    public bool TryDiscard(GameObject drink)
    {
        if (drink == null || drink != drinkInBin)
            return false;

        if (targetObject != null)
            targetObject.SetActive(true);

        Destroy(drink);
        drinkInBin = null;
        garbageBinAnimation?.Close();
        return true;
    }
}