using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class CharacterBubbleData
{
    [Header("文本里的角色名")]
    public string dialogName;

    [Header("场景里的对象名")]
    public string sceneObjectName;

    [Header("气泡生成坐标")]
    public Vector3 bubblePos;

    [Header("是否翻转气泡")]
    public bool reverse;

    [HideInInspector]
    public GameObject characterObj;
}

public class Plot_Dy : MonoBehaviour
{
    public Wentin wentin;
    public bool dialogFinished;

    private PlayerInfoList dialogData;
    private RecordLineNumber record;
    private bool dialogPaused;

    [Header("角色配置")]
    [SerializeField] private List<CharacterBubbleData> characterList = new List<CharacterBubbleData>();

    [Header("UI")]
    public GameObject DialogBox;

    [Header("文件路径")]
    public string path;

    [Header("对话序号")]
    public int x;

    private void Start()
    {
        LoadJson();
        FindCharactersInScene();
        record = GetComponent<RecordLineNumber>();

        if (record == null)
        {
            Debug.LogError("Plot_Dy 缺少 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        StartCoroutine(DialogRoutine());
    }

    private void FindCharactersInScene()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (CharacterBubbleData data in characterList)
        {
            data.characterObj = null;

            foreach (GameObject obj in allObjects)
            {
                if (obj.name != data.sceneObjectName)
                    continue;

                data.characterObj = obj;
                break;
            }

            if (data.characterObj == null)
                Debug.LogWarning($"场景中未找到角色对象: {data.sceneObjectName}");
        }
    }

    private void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);
        if (jsonFile == null)
        {
            dialogData = null;
            Debug.LogError($"JSON 读取失败：Resources/{path}", this);
            return;
        }

        dialogData = JsonUtility.FromJson<PlayerInfoList>(jsonFile.text);
        if (dialogData == null || dialogData.dialogList == null)
        {
            Debug.LogError($"JSON 格式错误：Resources/{path}", this);
            return;
        }

        Debug.Log($"成功读取 JSON，共有对话行数：{dialogData.dialogList.Count}");
    }

    public DialogUI CreateDialog(string text, CharacterBubbleData data, out Sequence sequence)
    {
        Canvas canvas = FindDialogCanvas();
        if (canvas == null)
        {
            sequence = DOTween.Sequence();
            Debug.LogError("未找到用于显示对话的 CanvasB", this);
            return null;
        }

        GameObject obj = Instantiate(DialogBox, canvas.transform);
        obj.transform.localPosition = data.bubblePos;
        obj.transform.localScale = new Vector3(data.reverse ? -0.1f : 0.1f, 0.1f, 0.1f);

        DialogUI dialog = obj.GetComponent<DialogUI>();
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = obj.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, 0.2f));
        sequence.Join(
            obj.transform.DOScale(
                new Vector3(data.reverse ? -1f : 1f, 0.5f, 1f),
                0.35f
            ).SetEase(Ease.OutBack)
        );

        sequence.OnComplete(() =>
        {
            Transform textTransform = obj.transform.Find("Text (TMP)");
            if (textTransform != null && data.reverse)
            {
                Vector3 textScale = textTransform.localScale;
                textScale.x *= -1;
                textTransform.localScale = textScale;
            }

            if (dialog != null)
                dialog.Setup(text);
        });

        return dialog;
    }

    private CharacterBubbleData GetCharacterData(string dialogName)
    {
        foreach (CharacterBubbleData data in characterList)
        {
            if (data.dialogName == dialogName)
                return data;
        }

        return null;
    }

    private IEnumerator DialogRoutine()
    {
        dialogFinished = false;

        if (dialogData == null || dialogData.dialogList == null)
            yield break;

        // Give every component one frame to establish an initial dialogue lock.
        yield return null;

        foreach (var line in dialogData.dialogList)
        {
            yield return new WaitUntil(() => !dialogPaused);
            if (line.Name == "default")
            {
                yield return ShowNarration(line.Speak, 1f);
                record.RecordLineNumberFunc(line.Num);
                continue;
            }

            CharacterBubbleData characterData = GetCharacterData(line.Name);
            if (characterData == null)
            {
                Debug.LogError($"未找到角色配置: {line.Name}", this);
                continue;
            }

            DialogUI dialog = CreateDialog(line.Speak, characterData, out Sequence animationSequence);
            if (dialog == null)
                continue;

            if (characterData.characterObj != null)
            {
                Wentin tool = GetComponent<Wentin>();
                if (tool != null)
                    tool.Start_And_Jump(characterData.characterObj);
            }

            yield return animationSequence.WaitForCompletion();

            yield return new WaitUntil(() =>
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    if (dialog.IsTyping())
                        dialog.CompleteLine();
                }

                return !dialog.IsTyping();
            });

            yield return new WaitUntil(() =>
                Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)
            );

            yield return new WaitUntil(() =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    return true;

                if (!Input.GetMouseButtonDown(0))
                    return false;

                return EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject();
            });

            Destroy(dialog.gameObject);
            yield return null;
            record.RecordLineNumberFunc(line.Num);
        }

        dialogFinished = true;
        Debug.Log("对话完成");
    }

    private IEnumerator ShowNarration(string text, float duration)
    {
        GameObject narration = FindNarrationObject();
        if (narration == null)
        {
            Debug.LogError("未找到 CanvasB/narration", this);
            yield return new WaitForSeconds(duration);
            yield break;
        }

        TMP_Text narrationText = narration.GetComponentInChildren<TMP_Text>(true);
        if (narrationText == null)
        {
            Debug.LogError("CanvasB/narration 下没有找到 TMP 文本组件", this);
            yield return new WaitForSeconds(duration);
            yield break;
        }

        narrationText.text = text;
        narrationText.maxVisibleCharacters = int.MaxValue;

        CanvasGroup narrationGroup = narration.GetComponent<CanvasGroup>();
        if (narrationGroup == null)
            narrationGroup = narration.AddComponent<CanvasGroup>();

        float fadeDuration = Mathf.Min(0.25f, duration * 0.5f);
        float holdDuration = Mathf.Max(0f, duration - fadeDuration * 2f);

        narrationGroup.alpha = 0f;
        narration.SetActive(true);

        Sequence narrationSequence = DOTween.Sequence();
        narrationSequence.Append(narrationGroup.DOFade(1f, fadeDuration).SetEase(Ease.Linear));
        narrationSequence.AppendInterval(holdDuration);
        narrationSequence.Append(narrationGroup.DOFade(0f, fadeDuration).SetEase(Ease.Linear));

        yield return narrationSequence.WaitForCompletion();

        narration.SetActive(false);
    }

    private static Canvas FindDialogCanvas()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == "CanvasB")
                return canvas;
        }

        return null;
    }

    private static GameObject FindNarrationObject()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name != "CanvasB")
                continue;

            Transform narration = canvas.transform.Find("narration");
            if (narration != null)
                return narration.gameObject;
        }

        return null;
    }

    public void ShowNarrationNotice(string text, float duration = 1f)
    {
        StartCoroutine(ShowNarration(text, duration));
    }

    public void SetDialogPaused(bool paused)
    {
        dialogPaused = paused;
    }

    public void SetBubblePosition(string dialogName, Vector3 position)// 暂停，不生成下一句
    {
        foreach (CharacterBubbleData data in characterList)
        {
            if (data.dialogName != dialogName)
                continue;

            data.bubblePos = position;
            return;
        }

        Debug.LogWarning($"未找到角色气泡配置: {dialogName}", this);
    }

    public void PlayNewPlot(string newPath)// 恢复，继续下一句
    {
        StopAllCoroutines();
        path = newPath;
        LoadJson();
        StartCoroutine(DialogRoutine());
    }
}
