using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveDon5 : MonoBehaviour
{
    private enum FaceState { Don, Cry, Excited, Pride, Pride2, Black, Worries, Oh }
    private static readonly int FaceStateId = Animator.StringToHash("FaceState");
    [SerializeField] private string followPath;
    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;
    private bool produceShown;
    private bool switchingToFollow;
    private bool sceneAdvanced;

    private void Start()
    {
        animator = GetComponent<Animator>();
        plot = GetComponent<Plot_Dy>();
        record = GetComponent<RecordLineNumber>();
        produce = FindProduceObject();
        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveDon5 needs Animator, Plot_Dy and RecordLineNumber.", this);
            enabled = false;
            return;
        }
        ChangeFace(FaceState.Don);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null) record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        switch (plot.x)
        {
            case 0: HandleOpening(line); break;
            case 1:
                ChangeFace(FaceState.Pride);
                if (line >= 1) PlayFollow();
                break;
            case 2:
                ChangeFace(line == 1 ? FaceState.Worries : FaceState.Oh);
                if (line >= 2) PlayFollow();
                break;
            case 3: HandleFollow(line); break;
        }
    }

    private void HandleOpening(int line)
    {
        if (line == 1) ChangeFace(FaceState.Excited);
        else if (line == 3) ChangeFace(FaceState.Cry);
        else if (line >= 5) StartCoroutine(ShowProduce());
    }

    private void HandleFollow(int line)
    {
        if (line == 1) ChangeFace(FaceState.Don);
        else if (line == 3) ChangeFace(FaceState.Oh);
        else if (line >= 5)
        {
            ChangeFace(FaceState.Pride2);
            AdvanceScene();
        }
    }

    private void PlayFollow()
    {
        if (switchingToFollow || string.IsNullOrEmpty(followPath)) return;
        switchingToFollow = true;
        plot.x = 3;
        plot.PlayNewPlot(followPath);
        switchingToFollow = false;
    }

    private void AdvanceScene()
    {
        if (sceneAdvanced) return;
        sceneAdvanced = true;
        if (TimeBool.Instance != null) TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
        else Debug.LogError("CharacterMoveDon5 cannot find TimeBool.", this);
    }

    private void ChangeFace(FaceState state)
    {
        if (animator.GetInteger(FaceStateId) != (int)state) animator.SetInteger(FaceStateId, (int)state);
    }

    private IEnumerator ShowProduce()
    {
        if (produceShown) yield break;
        produceShown = true;
        yield return new WaitForSeconds(0.3f);
        if (produce == null) yield break;
        produce.SetActive(true);
        CanvasGroup group = produce.GetComponent<CanvasGroup>();
        if (group == null) group = produce.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        group.DOFade(1f, 0.5f).OnComplete(() => { group.interactable = true; group.blocksRaycasts = true; });
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