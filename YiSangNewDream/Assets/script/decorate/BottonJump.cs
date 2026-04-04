using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BottonJump : MonoBehaviour
{
    [Header("晃动设置")]
    [Tooltip("上下晃动的幅度（像素）")]
    public float amplitude = 5f;

    [Tooltip("晃动频率（周期/秒）")]
    public float frequency = 2f;

    [Tooltip("是否启用晃动")]
    public bool enableFloat = true;

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private float originalY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // 记录初始位置（每次启用时重新获取，防止布局变化导致基准错误）
        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalY = originalAnchoredPosition.y;
    }

    private void Update()
    {
        if (!enableFloat) return;

        // 使用正弦波计算偏移量
        float offsetY = Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * frequency) * amplitude;

        Vector2 newPos = originalAnchoredPosition;
        newPos.y = originalY + offsetY;
        rectTransform.anchoredPosition = newPos;
    }

    private void OnDisable()
    {
        // 禁用时恢复原始位置，避免残留偏移
        if (rectTransform != null)
            rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void ResetPosition()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}
