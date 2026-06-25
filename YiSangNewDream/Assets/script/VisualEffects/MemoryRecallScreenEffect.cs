using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class MemoryRecallScreenEffect : MonoBehaviour
{
    private static readonly int EffectStrengthId = Shader.PropertyToID("_EffectStrength");

    private Material material;
    private float currentStrength;
    private float targetStrength;
    private float transitionDuration = 0.35f;

    public void Initialize(Shader shader, float duration)
    {
        transitionDuration = Mathf.Max(0.01f, duration);

        if (shader == null || material != null)
            return;

        material = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
    }

    public void SetTarget(float strength)
    {
        targetStrength = Mathf.Clamp01(strength);
    }

    private void Update()
    {
        currentStrength = Mathf.MoveTowards(
            currentStrength,
            targetStrength,
            Time.unscaledDeltaTime / transitionDuration
        );
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null || currentStrength <= 0.001f)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat(EffectStrengthId, currentStrength);
        Graphics.Blit(source, destination, material);
    }

    private void OnDestroy()
    {
        if (material != null)
            Destroy(material);
    }
}
