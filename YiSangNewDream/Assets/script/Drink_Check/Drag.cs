using UnityEngine;
using DG.Tweening;

public class DragAndReturn2D : MonoBehaviour
{
    private Vector3 originalPosition;
    public bool isDragging = false;
    private Vector3 originalScale;
    private GarbageBin garbageBin;

    private void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        AudioManager.Instance.PlaySFX("fiction", 1);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
            return;

        if (garbageBin != null && garbageBin.TryDiscard(gameObject))
        {
            isDragging = false;
            return;
        }

        transform.position = originalPosition;
        transform.DOKill();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack);
        isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GarbageBin bin = other.GetComponent<GarbageBin>();
        if (bin != null)
        {
            garbageBin = bin;
            return;
        }

        if (other.CompareTag("Cup"))
            isDragging = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<GarbageBin>() == garbageBin)
            garbageBin = null;
    }
}