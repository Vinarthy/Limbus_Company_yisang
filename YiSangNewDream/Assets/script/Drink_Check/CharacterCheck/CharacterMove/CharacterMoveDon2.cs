using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveDon2 : MonoBehaviour
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

    [Header("T/F 共用的后续剧情")]
    [SerializeField] private string followPath;

    private RecordLineNumber record;
    private Plot_Dy plot;
    private GameObject produce;
    private Animator animator;
    private bool switchingToFollow;

    private void Start()
    {
        animator = GetComponent<Animator>();
        plot = GetComponent<Plot_Dy>();
        record = GetComponent<RecordLineNumber>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveDon2 缺少 Animator、Plot_Dy 或 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        ChangeFace(FaceState.Don);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        switch (plot.x)
        {
            // 初始剧情：还没有进行饮品的 T/F 判定。
            case 0:
                HandleOpening(line);
                break;

            // T：判定成功剧情。
            // 成功分支的逐行表情和动作在这里修改。
            case 1:
                ChangeFace(FaceState.Pride);

                // T 文案第 1 行结束后，进入 T/F 共用的 Follow。
                if (line == 1)
                    PlayFollow();
                break;

            // F：判定失败剧情。
            // 失败分支的逐行表情和动作在这里修改。
            case 2:
                //F 剧情第 1 行：切换为 Excited 表情。
                //F 剧情其他所有行：切换为 Worries 表情。
                ChangeFace(line == 1 ? FaceState.Excited : FaceState.oh);

                // F 文案第 3 行结束后，进入 T/F 共用的 Follow。
                if (line == 3)
                    PlayFollow();
                break;

            // TF：T 和 F 汇合后的共用 Follow 剧情。
            case 3:
                HandleFollow(line);
                break;
        }
    }

    // 初始剧情（plot.x == 0）的逐行表情和动作。
    private void HandleOpening(int line)
    {
        switch (line)
        {
            case 1:
                ChangeFace(FaceState.Excited);
                break;
            case 2:
                ChangeFace(FaceState.Excited);
                break;
            case 3:
                ChangeFace(FaceState.Cry);
                break;
            case 7:
                ChangeFace(FaceState.Excited);
                break;
            case 10:
                ChangeFace(FaceState.Pride);
                break;
            case 12:
                StartCoroutine(WaitAndEnableProduce());
                break;
        }
    }

    // TF 共用 Follow（plot.x == 3）的逐行表情、动作和最终换幕。
    private void HandleFollow(int line)
    {
        switch (line)
        {
            case 1:
                ChangeFace(FaceState.Don);
                break;
            case 3:
                ChangeFace(FaceState.oh);
                break;
            case 5:
                ChangeFace(FaceState.Pride2);
                break;
            case 7:
                ChangeFace(FaceState.Excited);
                break;
            case 14:
                ChangeFace(FaceState.oh);
                break;
            case 15:
                ChangeFace(FaceState.oh);
                AdvanceScene();
                break;
        }
    }

    // T/F 各自播放结束后都会调用这里，切换到同一个 followPath。
    private void PlayFollow()
    {
        if (switchingToFollow || string.IsNullOrEmpty(followPath))
            return;

        switchingToFollow = true;
        plot.x = 3;
        plot.PlayNewPlot(followPath);
        switchingToFollow = false;
    }

    private void ChangeFace(FaceState state)
    {
        int value = (int)state;
        if (animator.GetInteger(FaceStateId) != value)
            animator.SetInteger(FaceStateId, value);
    }

    private static void AdvanceScene()
    {
        if (TimeBool.Instance != null)
            TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
    }

    private IEnumerator WaitAndEnableProduce()
    {
        yield return new WaitForSeconds(0.3f);

        if (produce == null)
        {
            Debug.LogWarning("未找到 Canvas 下的 produce", this);
            yield break;
        }

        produce.SetActive(true);

        CanvasGroup canvasGroup = produce.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = produce.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(1f, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
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