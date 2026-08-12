using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RecordLineNumber))]
[RequireComponent(typeof(Plot_Dy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovePose))]
public class CharacterMoveDongbai3 : MonoBehaviour
{
    public enum FaceState
    {
        Middle = 0,
        MiddleAngry = 1,
        MiddleScream = 2,
        MiddleSmile = 3,
        MiddleSorrow = 4,
        MiddleScare = 5,
        MiddleSmileNew = 6
    }

    public enum LiXiangFaceState
    {
        Default = 0,
        Angry = 1,
        Cough = 2,
        Cry = 3,
        Cry2 = 4,
        Embarrassed = 5,
        Scary = 6,
        Smile = 7,
        Smoke = 8,
        Sorrow = 9,
        PhoneCall = 10
    }

    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber record;
    private Plot_Dy plot;
    private CharacterMovePose movePose;
    private Animator dongbaiAnimator;
    private Animator liXiangAnimator;
    private GameObject produce;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        movePose = GetComponent<CharacterMovePose>();
        produce = FindProduceObject();

        if (record == null || plot == null || movePose == null)
        {
            Debug.LogError(
                "CharacterMoveDongbai3 缺少 RecordLineNumber、Plot_Dy 或 CharacterMovePose 组件。",
                this);
            enabled = false;
            return;
        }

        ResolveAnimators();
        ChangeFace(FaceState.Middle);
        ChangeLiXiangFace(LiXiangFaceState.Default);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        if (plot.x == 0)
            OnInitialLine(line);
        else if (plot.x == 1)
            OnCamelliaResultLine(line);
    }

    private void OnInitialLine(int line)
    {
        switch (line)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                StartCoroutine(WaitAndEnableProduce());
                break;
        }
    }

    private void OnCamelliaResultLine(int line)
    {
        switch (line)
        {
            case 1:
                ChangeFace(FaceState.MiddleSmileNew);
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                ChangeLiXiangFace(LiXiangFaceState.Cough);
                break;

            case 7:
                break;

            case 8:
                break;

            case 9:
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                break;

            case 10:
                break;

            case 11:
                break;

            case 12:
                break;

            case 13:
                break;

            case 14:
                break;

            case 15:
                break;

            case 16:
                break;

            case 17:
                break;

            case 18:
                break;

            case 19:
                break;

            case 20:
                break;

            case 21:
                break;

            case 22:
                break;

            case 23:
                break;

            case 24:
                break;

            case 25:
                break;

            case 26:
                break;

            case 27:
                break;

            case 28:
                break;

            case 29:
                break;

            case 30:
                if (TimeBool.Instance != null)
                    TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
                break;
        }
    }

    private void ChangeFace(FaceState faceState)
    {
        ChangeDongbaiFace(faceState);
    }

    public void ChangeDongbaiFace(FaceState faceState)
    {
        if (dongbaiAnimator == null)
            dongbaiAnimator = FindAnimator("冬柏");

        SetFaceState(dongbaiAnimator, (int)faceState);
    }

    public void ChangeLiXiangFace(LiXiangFaceState faceState)
    {
        if (liXiangAnimator == null)
            liXiangAnimator = FindAnimator("李箱");

        SetFaceState(liXiangAnimator, (int)faceState);
    }

    private void ResolveAnimators()
    {
        dongbaiAnimator = FindAnimator("冬柏");
        liXiangAnimator = FindAnimator("李箱");
    }

    private Animator FindAnimator(string dialogName)
    {
        Transform target = plot.GetCharacterTransform(dialogName);
        if (target == null)
            return null;

        if (!target.TryGetComponent(out Animator targetAnimator))
        {
            Debug.LogWarning($"角色缺少 Animator: {dialogName}", target);
            return null;
        }

        return targetAnimator;
    }

    private static void SetFaceState(Animator targetAnimator, int faceState)
    {
        if (targetAnimator != null && targetAnimator.GetInteger(FaceStateId) != faceState)
            targetAnimator.SetInteger(FaceStateId, faceState);
    }

    private IEnumerator WaitAndEnableProduce()
    {
        yield return new WaitForSeconds(0.3f);
        if (produce == null)
            yield break;

        produce.SetActive(true);
        CanvasGroup group = produce.GetComponent<CanvasGroup>();
        if (group == null)
            group = produce.AddComponent<CanvasGroup>();

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        group.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        });
    }

    private static GameObject FindProduceObject()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name != "Canvas")
                continue;

            Transform target = canvas.transform.Find("produce");
            if (target != null)
                return target.gameObject;
        }

        return null;
    }
}