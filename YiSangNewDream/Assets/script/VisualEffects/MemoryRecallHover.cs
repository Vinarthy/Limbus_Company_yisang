using UnityEngine;

public class MemoryRecallHover : MonoBehaviour
{
    [SerializeField] private Shader distortionShader;
    [SerializeField, Range(0f, 1f)] private float effectStrength = 0.85f;
    [SerializeField, Min(0.01f)] private float transitionDuration = 0.35f;

    private MemoryRecallScreenEffect screenEffect;

    private void OnMouseEnter()
    {
        if (TryGetScreenEffect())
            screenEffect.SetTarget(effectStrength);
    }

    private void OnMouseExit()
    {
        StopEffect();
    }

    private void OnDisable()
    {
        StopEffect();
    }

    private bool TryGetScreenEffect()
    {
        if (screenEffect != null)
            return true;

        Camera targetCamera = Camera.main;
        if (targetCamera == null)
            targetCamera = FindObjectOfType<Camera>();

        if (targetCamera == null)
        {
            Debug.LogWarning("MemoryRecallHover：场景中没有找到 Camera。", this);
            return false;
        }

        screenEffect = targetCamera.GetComponent<MemoryRecallScreenEffect>();
        if (screenEffect == null)
            screenEffect = targetCamera.gameObject.AddComponent<MemoryRecallScreenEffect>();

        screenEffect.Initialize(distortionShader, transitionDuration);
        return true;
    }

    private void StopEffect()
    {
        if (screenEffect != null)
            screenEffect.SetTarget(0f);
    }
}
