using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RecordLineNumber))]
[RequireComponent(typeof(Plot_Dy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovePose))]
public class CharacterMoveDon6 : MonoBehaviour
{
    public enum FaceState
    {
        Don = 0,
        Cry = 1,
        Excited = 2,
        Pride = 3,
        Pride2 = 4,
        Black = 5,
        Worries = 6,
        Oh = 7,
        FallDown = 8,
        Guita1 = 9,
        Guita2 = 10
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
    private Animator donQuixoteAnimator;
    private Animator liXiangAnimator;
    private GameObject produce;
    private bool produceShown;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        movePose = GetComponent<CharacterMovePose>();
        produce = FindProduceObject();

        if (record == null || plot == null || movePose == null)
        {
            Debug.LogError(
                "CharacterMoveDon6 缺少 RecordLineNumber、Plot_Dy 或 CharacterMovePose 组件。",
                this);
            enabled = false;
            return;
        }

        ResolveAnimators();
        ChangeFace(FaceState.Don);
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
            OnIceCreamResultLine(line);
    }

    private void OnInitialLine(int line)
    {
        switch (line)
        {
            case 1:
                ChangeFace(FaceState.Pride2);
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                break;

            case 2:
                break;

            case 3:
                ChangeLiXiangFace(LiXiangFaceState.Embarrassed);
                break;

            case 4:
                ChangeFace(FaceState.Cry);
                break;

            case 5:
                break;

            case 6:
                break;

            case 7:
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                ChangeFace(FaceState.Oh);
                break;

            case 8:
                break;

            case 9:
                StartCoroutine(WaitAndEnableProduce());
                break;
        }
    }

    private void OnIceCreamResultLine(int line)
    {
        switch (line)
        {
            case 1:
                ChangeFace(FaceState.Excited);
                break;

            case 2:
                //来个惜春堂
                ChangeFace(FaceState.Black);
                break;

            case 3:
                //小金来
                movePose?.MoveByX(11f, 0.5f, "地雷小金", new Vector3(85.2f, 9.6f, 0f));
                break;

            case 4:
                //堂恢复正常
                ChangeFace(FaceState.Don);
                break;

            case 5:
                SceneManager.LoadScene("ED");
                break;
        }
    }

    private void ChangeFace(FaceState faceState)
    {
        ChangeDonQuixoteFace(faceState);
    }

    public void ChangeDonQuixoteFace(FaceState faceState)
    {
        if (donQuixoteAnimator == null)
            donQuixoteAnimator = FindAnimator("堂吉诃德");

        SetFaceState(donQuixoteAnimator, (int)faceState);
    }

    public void ChangeLiXiangFace(LiXiangFaceState faceState)
    {
        if (liXiangAnimator == null)
            liXiangAnimator = FindAnimator("李箱");

        SetFaceState(liXiangAnimator, (int)faceState);
    }

    private void ResolveAnimators()
    {
        donQuixoteAnimator = FindAnimator("堂吉诃德");
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
        if (produceShown)
            yield break;

        produceShown = true;
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
        foreach (Canvas canvas in FindObjectsOfType<Canvas>(true))
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