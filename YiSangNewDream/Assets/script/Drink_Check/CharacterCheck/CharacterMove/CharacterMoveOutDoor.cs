using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(RecordLineNumber))]
[RequireComponent(typeof(CharacterMovePose))]
[RequireComponent(typeof(PlayableDirector))]
public class CharacterMoveOutDoor : MonoBehaviour
{
    private struct TimelineObjectState
    {
        public GameObject target;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public bool activeSelf;

        public TimelineObjectState(Transform targetTransform)
        {
            target = targetTransform.gameObject;
            localPosition = targetTransform.localPosition;
            localRotation = targetTransform.localRotation;
            localScale = targetTransform.localScale;
            activeSelf = target.activeSelf;
        }
    }
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

    public enum DonQuixoteFaceState
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

    private Animator dongbaiAnimator;
    private Animator donQuixoteAnimator;
    private Animator liXiangAnimator;
    private RecordLineNumber record;
    private Plot_Dy plot;
    private CharacterMovePose movePose;
    private PlayableDirector timelineDirector;
    private Coroutine case4TimelineRoutine;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        movePose = GetComponent<CharacterMovePose>();
        timelineDirector = GetComponent<PlayableDirector>();

        if (record == null || plot == null || movePose == null || timelineDirector == null)
        {
            Debug.LogError("CharacterMoveOutDoor 缺少 RecordLineNumber、Plot_Dy、CharacterMovePose 或 PlayableDirector 组件。", this);
            enabled = false;
            return;
        }

        ResolveAnimators();
        ChangeFace(FaceState.Middle);
        ChangeDonQuixoteFace(DonQuixoteFaceState.FallDown);
        movePose?.MoveByY(-2f,0.1f,"堂吉诃德", new Vector3(-73f, -29f, 0f));
        ChangeLiXiangFace(LiXiangFaceState.Scary);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (case4TimelineRoutine != null)
            plot?.SetDialogPaused(false);

        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }
    //香飘飘热饮
    private void OnLineChanged(int line)
    {
        switch (line)
        {
            case 1:
                movePose?.MoveByY(2f, 0.1f, "堂吉诃德", new Vector3(-73f, -29f, 0f));
                ChangeDonQuixoteFace(DonQuixoteFaceState.Worries);
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                //zwei的来，然后来个精灵球动画，然后再说话（）
                PlayCase4Timeline();
                break;

            case 5:
                ChangeDonQuixoteFace(DonQuixoteFaceState.Excited);
                break;

            case 6:
                break;

            case 7:
                break;

            case 8:
                break;

            case 9:
                break;

            case 10:
                ChangeDonQuixoteFace(DonQuixoteFaceState.FallDown);
                movePose?.MoveByY(-2f, 0.1f, "堂吉诃德", new Vector3(-73f, -29f, 0f));
                ChangeLiXiangFace(LiXiangFaceState.Embarrassed);
                break;

            case 11:
                break;

            case 12:
                break;

            case 13:
                break;

            case 14:
                ChangeLiXiangFace(LiXiangFaceState.Cough);
                break;

            case 15:
                break;

            case 16:
                ChangeFace(FaceState.MiddleSmileNew);
                break;

            case 17:
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                break;

            case 18:

                break;
            //没有人是重要的，别人都把我当小丑看呢
            case 19:
                ChangeLiXiangFace(LiXiangFaceState.Cry2);
                break;

            case 20:
                ChangeFace(FaceState.MiddleSorrow);
                break;

            case 21:
                ChangeFace(FaceState.MiddleSmileNew);
                ChangeLiXiangFace(LiXiangFaceState.Smile);
                break;

            case 22:
                break;

            case 23:
                break;

            case 24:
                break;
        }
    }

    private void PlayCase4Timeline()
    {
        if (case4TimelineRoutine != null)
            return;

        if (timelineDirector.playableAsset == null)
        {
            Debug.LogError("CharacterMoveOutDoor 的 PlayableDirector 未配置 Timeline。", this);
            return;
        }

        case4TimelineRoutine = StartCoroutine(PlayCase4TimelineRoutine());
    }

    private IEnumerator PlayCase4TimelineRoutine()
    {
        plot.SetDialogPaused(true);

        timelineDirector.extrapolationMode = DirectorWrapMode.Hold;
        timelineDirector.time = 0;
        timelineDirector.Play();

        while (timelineDirector.state == PlayState.Playing &&
               timelineDirector.time < timelineDirector.duration)
        {
            yield return null;
        }

        timelineDirector.time = timelineDirector.duration;
        timelineDirector.Evaluate();

        List<TimelineObjectState> finalStates = CaptureTimelineObjectStates();
        timelineDirector.Stop();
        RestoreTimelineObjectStates(finalStates);

        case4TimelineRoutine = null;
        plot.SetDialogPaused(false);
    }

    private List<TimelineObjectState> CaptureTimelineObjectStates()
    {
        List<TimelineObjectState> states = new List<TimelineObjectState>();
        List<GameObject> capturedObjects = new List<GameObject>();

        foreach (PlayableBinding output in timelineDirector.playableAsset.outputs)
        {
            Object binding = timelineDirector.GetGenericBinding(output.sourceObject);
            GameObject rootObject = binding as GameObject;

            if (rootObject == null && binding is Component component)
                rootObject = component.gameObject;

            if (rootObject == null)
                continue;

            foreach (Transform target in rootObject.GetComponentsInChildren<Transform>(true))
            {
                if (capturedObjects.Contains(target.gameObject))
                    continue;

                capturedObjects.Add(target.gameObject);
                states.Add(new TimelineObjectState(target));
            }
        }

        return states;
    }

    private static void RestoreTimelineObjectStates(List<TimelineObjectState> states)
    {
        foreach (TimelineObjectState state in states)
        {
            if (state.target == null)
                continue;

            Transform target = state.target.transform;
            target.localPosition = state.localPosition;
            target.localRotation = state.localRotation;
            target.localScale = state.localScale;
        }

        foreach (TimelineObjectState state in states)
        {
            if (state.target != null && state.target.activeSelf != state.activeSelf)
                state.target.SetActive(state.activeSelf);
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

    public void ChangeDonQuixoteFace(DonQuixoteFaceState faceState)
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
        dongbaiAnimator = FindAnimator("冬柏");
        donQuixoteAnimator = FindAnimator("堂吉诃德");
        liXiangAnimator = FindAnimator("李箱");
    }

    private static void SetFaceState(Animator targetAnimator, int faceState)
    {
        if (targetAnimator != null && targetAnimator.GetInteger(FaceStateId) != faceState)
            targetAnimator.SetInteger(FaceStateId, faceState);
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
}
