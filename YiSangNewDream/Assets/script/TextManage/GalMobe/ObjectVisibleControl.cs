using UnityEngine;

public class ObjectVisibleControl : MonoBehaviour
{
    public GameObject targetObject;

    public void SetVisible(bool visible)
    {
        if (targetObject != null && targetObject.activeSelf != visible)
            targetObject.SetActive(visible);
    }

    public void Show()
    {
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
    }
}