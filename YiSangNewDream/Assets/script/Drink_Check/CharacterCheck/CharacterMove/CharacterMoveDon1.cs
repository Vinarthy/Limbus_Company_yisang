using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveDon1 : MonoBehaviour
{
    private enum FaceState
    {
        Don = 0,
        Cry = 1,
        Excited = 2,
        Pride = 3,
        Pride2 = 4,
        Black = 5,
        Worries = 6,
            oh = 7
    }

    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber re;
    private Plot_Dy DialogNum;
    private GameObject targetGameObject;
    private Animator anim;
    private Tween moveTween;

    private static readonly Vector3 IntroPosition = new Vector3(-12.4f, -1.18f, 0f);
    private static readonly Vector3 NormalPosition = new Vector3(-4.31f, -1.18f, 0f);
    private static readonly Vector3 IntroBubblePosition = new Vector3(-175f, 0f, 0f);
    private const float ReturnMoveDuration = 0.8f;

    private void Awake()
    {
        DialogNum = GetComponent<Plot_Dy>();
        transform.position = IntroPosition;

        if (DialogNum != null)
        {
            DialogNum.SetBubblePosition("堂吉诃德", IntroBubblePosition);
            DialogNum.SetBubblePosition("李箱", Vector3.zero);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        DialogNum = GetComponent<Plot_Dy>();
        re = GetComponent<RecordLineNumber>();
        targetGameObject = FindProduceObject();

        if (targetGameObject == null)
            Debug.LogError("未找到 Canvas 下的 produce", this);

        if (re == null || DialogNum == null || anim == null)
        {
            Debug.LogError("CharacterMoveDon1 缺少 Animator、Plot_Dy 或 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        re.OnLineChanged += OnLineChanged;
    }

    void OnDestroy()
    {
        moveTween?.Kill();

        if (re != null)
            re.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        int num = DialogNum.x;
        if (num == 0)
        {
            switch (line)
            {
                case 1:
                    ChangeFace(FaceState.oh);
                    Debug.Log("执行第1行逻辑");
                    break;

                case 3:
                    ChangeFace(FaceState.oh);
                    Debug.Log("执行第3行动作");
                    function();
                    ReturnToNormalPosition();
                    break;
                case 5:
                    ChangeFace(FaceState.Don);
                    Debug.Log("执行第1行逻辑");
                    break;

                case 13:
                    ChangeFace(FaceState.Cry);
                    break;
                case 14:
                    ChangeFace(FaceState.Don);
                    StartCoroutine(WaitAndEnable());
                    break;
            }
        }
        else if (num == 1)
        {
            switch (line)
            {
                case 1:
                    Debug.Log("这是判定成功了");
                    break;
                case 3:
                    Debug.Log("更新幕");
                    TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
                    break;
            }
        }
        else if (num == 2)
        {
            switch (line)
            {
                case 1:
                    ChangeFace(FaceState.oh);
                    Debug.Log("这是判定失败了");
                    break;
                case 2:
                    Debug.Log("更新幕");
                    ChangeFace(FaceState.Pride2);
                    TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
                    break;
                case 3:
                    ChangeFace(FaceState.oh);
                    break;
            }
        }
    }

    private void ReturnToNormalPosition()
    {
        if (DialogNum == null)
            return;

        DialogNum.SetDialogPaused(true);
        moveTween?.Kill();

        moveTween = transform
            .DOMove(NormalPosition, ReturnMoveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (DialogNum == null)
                    return;

                DialogNum.SetBubblePosition("堂吉诃德", Vector3.zero);
                DialogNum.SetDialogPaused(false);
            });
    }

    private void ChangeFace(FaceState state)
    {
        int value = (int)state;
        if (anim.GetInteger(FaceStateId) != value)
            anim.SetInteger(FaceStateId, value);
    }

    // 这个放日期增加逻辑
    // 在这里写入场景以及日期更新逻辑
    void function()
    {
        Debug.Log("角色动作触发");
    }

    IEnumerator WaitAndEnable()
    {
        yield return new WaitForSeconds(0.3f);

        if (targetGameObject == null)
            yield break;

        targetGameObject.SetActive(true);

        CanvasGroup cg = targetGameObject.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = targetGameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        cg.DOFade(1f, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            });
    }

    private GameObject FindProduceObject()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == "CanvasB")
                continue;

            if (canvas.name != "Canvas")
                continue;

            Transform target = canvas.transform.Find("produce");
            if (target != null)
                return target.gameObject;
        }

        return null;
    }
}