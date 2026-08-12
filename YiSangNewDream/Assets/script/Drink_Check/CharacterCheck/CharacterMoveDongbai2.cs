using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RecordLineNumber))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterMovePose))]
public class CharacterMoveDongbai2 : MonoBehaviour
{
    public enum FaceState
    {
        Middle = 0,
        MiddleAngry = 1,
        MiddleScream = 2,
        MiddleSmile = 3,
        MiddleSorrow = 4,
        MiddleScare = 5
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
        Oh = 7
    }

    public enum DongrangFaceState
    {
        Default = 0,
        Embarrassed = 1,
        Smile = 2,
        Strict = 3
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
    private Animator dongrangAnimator;
    private Animator liXiangAnimator;

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Transform cowTransform;
    private Sequence case34Sequence;

    //这个里面状态机非常多了
    private CharacterMovePose movePose;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();

        movePose = GetComponent<CharacterMovePose>();

        ResolveAnimators();

        ChangeFace(FaceState.Middle);
        ChangeDonQuixoteFace(DonQuixoteFaceState.Don);
        ChangeDongrangFace(DongrangFaceState.Default);
        ChangeLiXiangFace(LiXiangFaceState.Default);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        case34Sequence?.Kill();
        plot?.SetDialogPaused(false);

        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        switch (line) { //状态机咋切换的来着？
            case 1:
                ChangeFace(FaceState.MiddleSorrow);
                break;

            case 2:
                MoveByX(6.5f, 0.4f, "冬柏", new Vector3(-63f, -16.95342f, 0f));
                break;

            case 3:
                
                break;

            case 4:
                break;

            case 5:
                //给箱子留个预留
                break;

            case 6:
                break;

            case 7:
                break;

            case 8:
                MoveByX(10.2f, 0.8f, "东朗", new Vector3(62.4f, -16.95342f, 0f));
                ChangeDongrangFace(DongrangFaceState.Default);
                break;

            case 9:
                ChangeFace(FaceState.MiddleAngry);
                break;

            case 10:
                ChangeDongrangFace(DongrangFaceState.Embarrassed);
                break;

            case 11:
                
                break;

            case 12:
                break;

            case 13:
                ChangeDongrangFace(DongrangFaceState.Smile);
                break;

            case 14:
                ChangeFace(FaceState.MiddleSorrow);
                break;

            case 15:
                
                break;

            case 16:
                break;

            case 17:
                ChangeDongrangFace(DongrangFaceState.Default);
                break;

            case 18:
                break;

            case 19:
                break;

            case 20:
                break;

            case 21:
                ChangeFace(FaceState.Middle);
                break;

            case 22:
                ChangeDongrangFace(DongrangFaceState.Smile);
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
                ChangeDongrangFace(DongrangFaceState.Default);
                break;

            case 29:
                // 第29行由 Plot_Dy 从 JSON 读取并显示在 CanvasB/narration。
                break;

            case 30:
                break;

            case 31:
                MoveByX(-14f, 3f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                break;

            case 32:
                //有牛
                ChangeDongrangFace(DongrangFaceState.Smile);
                break;

            case 33:
                ChangeDongrangFace(DongrangFaceState.Strict);
                MoveByX(0.4f, 1.5f, "东朗", new Vector3(62.4f, -16.95342f, 0f));
                break;

            case 34:
                ChangeFace(FaceState.MiddleScare);
                PlayCase34Animation();
                break;

            case 35:
                
                break;

            case 36:
                //好躲朋友好躲
                MoveByX(-10f, 0.8f, "冬柏", new Vector3(5.2f, -16.95342f, 0f));
                movePose?.MoveByY(-4f,1.2f,"李箱",new Vector3(35f, -151f, 0f));
                break;

            case 37:
                //东柏换图层，李箱蹲
                movePose?.SetSortingLayer("冬柏", 5);
                movePose?.MoveByY(-5.8f, 1.2f, "冬柏", new Vector3(35f, -151f, 0f));
                MoveByX(18f, 0.8f, "冬柏", new Vector3(263.8f, -151f, 0f));
                MoveByX(-24f, 0.8f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                break;

            case 38:
                movePose?.FlipX("牛哥");
                MoveByX(32f, 0.8f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                break;

            case 39:
                movePose?.FlipX("牛哥");
                MoveByX(-29f, 0.8f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                break;

            case 40:

                movePose?.FlipX("牛哥");
                MoveByX(29f, 0.8f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                //这里要留一个箱子打电话的状态机
                break;

            case 41:

                break;

            case 42:
                break;

            case 43:
                break;

            case 44:
                ChangeFace(FaceState.MiddleScream);
                break;

            case 45:
                
                MoveByX(8f, 1.2f, "堂吉诃德", new Vector3(0f, 0f, 0f));
                ChangeDonQuixoteFace(DonQuixoteFaceState.Worries);
                break;

            case 46:
                //你的堂来了

                break;

            case 47:
                movePose?.FlipX("牛哥");
                MoveByX(-44f, 0.6f, "牛哥", new Vector3(62.4f, -16.95342f, 0f));
                movePose?.MoveByY(-5.8f, 0.3f, "堂吉诃德", new Vector3(35f, -151f, 0f));
                break;

            case 48://牛来，堂躲
                movePose?.MoveByY(5.8f, 0.3f, "堂吉诃德", new Vector3(-35f, 0f, 0f));
                ChangeDonQuixoteFace(DonQuixoteFaceState.Pride2);
                break;

            case 49://堂起身，换表情
                ChangeDonQuixoteFace(DonQuixoteFaceState.Worries);
                break;

            case 50:
                
                break;

            case 51:
                break;

            case 52:
                break;
            case 53:
                //直接换场景，换幕，不回退了（）
                //前面的搞一个自动退会场景的吧（）
                SceneManager.LoadScene("OutDoor");

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

    public void ChangeDonQuixoteFace(DonQuixoteFaceState faceState)
    {
        if (donQuixoteAnimator == null)
            donQuixoteAnimator = FindAnimator("堂吉诃德");

        SetFaceState(donQuixoteAnimator, (int)faceState);
    }

    public void ChangeDongrangFace(DongrangFaceState faceState)
    {
        if (dongrangAnimator == null)
            dongrangAnimator = FindAnimator("东朗");

        SetFaceState(dongrangAnimator, (int)faceState);
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
        dongrangAnimator = FindAnimator("东朗");
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

    private void PlayCase34Animation()
    {
        if (cowTransform == null)
            cowTransform = plot?.GetCharacterTransform("牛哥");

        if (cowTransform == null)
        {
            Debug.LogError("CharacterMoveDongbai2 未找到牛哥对象 cow(Clone)。", this);
            PlayCase34DongbaiMove();
            return;
        }

        case34Sequence?.Kill();
        plot?.SetDialogPaused(true);
        float cowStartX = cowTransform.position.x;

        case34Sequence = DOTween.Sequence()
            .Append(cowTransform.DOMoveX(cowStartX - 1f, 0.2f).SetEase(Ease.InOutQuad))
            .Append(cowTransform.DOMoveX(cowStartX, 0.2f).SetEase(Ease.InOutQuad))
            .Append(cowTransform.DOMoveX(cowStartX - 1f, 0.2f).SetEase(Ease.InOutQuad))
            .AppendCallback(PlayCase34DongbaiMove)
            .AppendInterval(1.2f)
            .OnComplete(() =>
            {
                case34Sequence = null;
                plot?.SetDialogPaused(false);
            });
    }

    private void PlayCase34DongbaiMove()
    {
        movePose?.RotateByZ(63f, 0.6f, "东朗");
        movePose?.MoveByY(
            -6.8f,
            0.6f,
            "东朗",
            new Vector3(62.4f, -16.95342f, 0f));
    }

    private void MoveByX(
        float distance,
        float moveDuration,
        string dialogName,
        Vector3 bubblePosition)
    {
        movePose.MoveByX(distance, moveDuration, dialogName, bubblePosition);
    }
}
