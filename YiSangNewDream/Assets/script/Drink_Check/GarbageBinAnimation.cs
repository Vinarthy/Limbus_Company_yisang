using System.Collections;
using UnityEngine;

public class GarbageBinAnimation : MonoBehaviour
{
    [Header("垃圾桶盖")]
    public GameObject lidObject;

    [SerializeField] private float openAngle = -70f;
    [SerializeField] private float rotationSpeed = 240f;

    private bool isOpen;
    private Coroutine closeRoutine;

    private void Update()
    {
        if (lidObject == null)
            return;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, isOpen ? openAngle : 0f);
        lidObject.transform.localRotation = Quaternion.RotateTowards(
            lidObject.transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void OpenForDuration(float duration)
    {
        Open();

        if (closeRoutine != null)
            StopCoroutine(closeRoutine);

        closeRoutine = StartCoroutine(CloseAfterDuration(duration));
    }

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    private IEnumerator CloseAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Close();
        closeRoutine = null;
    }
}