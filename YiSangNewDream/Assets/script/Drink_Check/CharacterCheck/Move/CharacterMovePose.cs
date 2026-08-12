using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plot_Dy))]
public class CharacterMovePose : MonoBehaviour
{
    [SerializeField] private Ease ease = Ease.InOutQuad;

    private Plot_Dy plot;
    private readonly Dictionary<Transform, Tween> moveXTweens = new Dictionary<Transform, Tween>();
    private readonly Dictionary<Transform, Tween> moveYTweens = new Dictionary<Transform, Tween>();
    private readonly Dictionary<Transform, Tween> rotateTweens = new Dictionary<Transform, Tween>();

    private void Awake()
    {
        plot = GetComponent<Plot_Dy>();
    }

    private void OnDestroy()
    {
        KillAllTweens(moveXTweens);
        KillAllTweens(moveYTweens);
        KillAllTweens(rotateTweens);
    }

    public void MoveByX(
        float distance,
        float moveDuration,
        string dialogName,
        Vector3 bubblePosition)
    {
        plot.SetBubblePosition(dialogName, bubblePosition);
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        KillTween(moveXTweens, target);
        moveXTweens[target] = target
            .DOMoveX(target.position.x + distance, Mathf.Max(0f, moveDuration))
            .SetEase(ease);
    }

    public void MoveByY(
        float distance,
        float moveDuration,
        string dialogName,
        Vector3 bubblePosition)
    {
        plot.SetBubblePosition(dialogName, bubblePosition);
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        KillTween(moveYTweens, target);
        moveYTweens[target] = target
            .DOMoveY(target.position.y + distance, Mathf.Max(0f, moveDuration))
            .SetEase(ease);
    }

    public void RotateByZ(float angle, float rotateDuration, string dialogName)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        KillTween(rotateTweens, target);
        rotateTweens[target] = target
            .DORotate(
                new Vector3(0f, 0f, angle),
                Mathf.Max(0f, rotateDuration),
                RotateMode.LocalAxisAdd)
            .SetEase(ease);
    }

    public void SetSortingLayer(string dialogName, int sortingOrder)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        if (!target.TryGetComponent(out SpriteRenderer targetRenderer))
        {
            Debug.LogWarning($"角色缺少 SpriteRenderer: {dialogName}", target);
            return;
        }

        targetRenderer.sortingOrder = sortingOrder;
    }

    public void FlipX(string dialogName)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        Vector3 scale = target.localScale;
        scale.x *= -1f;
        target.localScale = scale;
    }

    public void StopMove(string dialogName)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return;

        KillTween(moveXTweens, target);
        KillTween(moveYTweens, target);
    }

    public void StopRotation(string dialogName)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target != null)
            KillTween(rotateTweens, target);
    }

    private static void KillTween(Dictionary<Transform, Tween> tweens, Transform target)
    {
        if (!tweens.TryGetValue(target, out Tween tween))
            return;

        tween?.Kill();
        tweens.Remove(target);
    }

    private static void KillAllTweens(Dictionary<Transform, Tween> tweens)
    {
        foreach (Tween tween in tweens.Values)
            tween?.Kill();

        tweens.Clear();
    }
}
//characterMovePose?.MoveByX(3f,1.2f,"ʩ����", *Plot_Dy�е�dialogName* new Vector3(200f, 100f, 0f) // �����Ի���λ��);
//����Ϊ���÷���
//����Ҳ�п�������