using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveRodio1 : MonoBehaviour
{
    private static readonly int FaceStateId = Animator.StringToHash("FaceState");
    private const string FollowTaskPath = "Dialog/ProduceTask/taskshi";
    [SerializeField] private string follow1Path;

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;
    private TaskControl taskControl;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        animator = GetComponent<Animator>();
        produce = FindProduceObject();
        taskControl = GetComponent<TaskControl>();
        if (record == null || plot == null || animator == null)
        {
            enabled = false;
            return;
        }

        animator.SetInteger(FaceStateId, 0);
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
            case 0:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 0);
                        break;

                    case 2:
                        animator.SetInteger(FaceStateId, 0);
                        ShowProduce();
                        break;
                }
                break;

            case 1:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 0);
                        PlayFollow1();
                        break;
                }
                break;

            case 2:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 1);
                        PlayFollow1();
                        break;
                }
                break;

            case 3:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 0);
                        break;

                    case 2:
                        animator.SetInteger(FaceStateId, 0);
                        break;

                    case 3:
                        animator.SetInteger(FaceStateId, 0);
                        break;

                    case 4:
                        animator.SetInteger(FaceStateId, 2);
                        break;

                    case 5:
                        animator.SetInteger(FaceStateId, 3);
                        ShowProduce();
                        break;
                }
                break;

            case 4:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 3);
                        ShowProduce();
                        break;
                }
                break;

            case 5:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 3);
                        ShowProduce();
                        break;
                }
                break;

            case 6:
                switch (line)
                {
                    case 1:
                        animator.SetInteger(FaceStateId, 1);
                        break;

                    case 2:
                        animator.SetInteger(FaceStateId, 1);
                        break;

                    case 3:
                        animator.SetInteger(FaceStateId, 1);
                        break;

                    case 4:
                        animator.SetInteger(FaceStateId, 1);
                        if (TimeBool.Instance != null)
                            TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
                        break;
                }
                break;
        }
    }

    private void PlayFollow1()
    {
        if (taskControl != null)
            taskControl.SetTaskPath(FollowTaskPath);

        plot.x = 3;
        plot.PlayNewPlot(follow1Path);
    }

    private void ShowProduce()
    {
        StartCoroutine(WaitAndEnableProduce());
    }

    private IEnumerator WaitAndEnableProduce()
    {
        yield return new WaitForSeconds(0.3f);
        if (produce == null) yield break;

        produce.SetActive(true);
        CanvasGroup group = produce.GetComponent<CanvasGroup>();
        if (group == null) group = produce.AddComponent<CanvasGroup>();
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
            if (canvas.name != "Canvas") continue;
            Transform target = canvas.transform.Find("produce");
            if (target != null) return target.gameObject;
        }
        return null;
    }
}
