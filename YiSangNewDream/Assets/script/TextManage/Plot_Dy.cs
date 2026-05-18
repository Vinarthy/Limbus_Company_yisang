using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

//附加：我认为这个函数应该是控制角色的，并调用上述的函数，不应该耦合
//再附加：这个脚本主要是剧情走向控制来着应该
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
    // Start is called before the first frame update
    public Wentin wentin;
    public bool dialogFinished = false;
    private PlayerInfoList dialogData;//这个好像不用挂（
    private RecordLineNumber record;

    [Header("角色配置")]
    [SerializeField]
    private List<CharacterBubbleData> characterList = new List<CharacterBubbleData>();

    [Header("UI")]
    public GameObject DialogBox;

    [Header("文件路径")]
    public string path;
    [Header("对话序号")]
    public int x=0;//默认为0，根据不同的对话序号引用不同的动作，直接改在MoveControl1里面
    void Start()
    {
        LoadJson();
        FindCharactersInScene();
        wentin.SceneMiddleStart();
        record = GetComponent<RecordLineNumber>();
        StartCoroutine(DialogRoutine());
    }
    private void FindCharactersInScene()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (var data in characterList)
        {
            foreach (var obj in allObjects)
            {

                if (obj.name == data.sceneObjectName)
                {
                    data.characterObj = obj;
                    break;
                }
            }

            if (data.characterObj == null)
            {
                Debug.LogWarning($"场景中未找到角色对象: {data.sceneObjectName}");
            }
        }
    }
    // Update is called once per frame
    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);//Dialog/test
        if (jsonFile != null)
        {
            dialogData = JsonUtility.FromJson<PlayerInfoList>(jsonFile.text);
            Debug.Log("成功读取 JSON，共有对话行数：" + dialogData.dialogList.Count);
        }
        else
        {
            Debug.LogError("JSON 读取失败！");
        }
    }
    public DialogUI CreateDialog(string text, CharacterBubbleData data, out Sequence seq)
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject obj = Instantiate(DialogBox, canvas.transform);

        obj.transform.localPosition = data.bubblePos;

        Vector3 scale = new Vector3(
            data.reverse ? -0.1f : 0.1f,
            0.1f,
            0.1f
        );

        obj.transform.localScale = scale;

        DialogUI dialog = obj.GetComponent<DialogUI>();

        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = obj.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, 0.2f));

        Vector3 targetScale = new Vector3(
            data.reverse ? -1f : 1f,
            0.5f,
            1f
        );

        seq.Join(
            obj.transform.DOScale(targetScale, 0.35f)
            .SetEase(Ease.OutBack)
        );

        seq.OnComplete(() =>
        {
            Transform textTransform = obj.transform.Find("Text (TMP)");

            if (textTransform != null && data.reverse)
            {
                Vector3 textScale = textTransform.localScale;

                textScale.x *= -1;

                textTransform.localScale = textScale;
            }

            dialog.Setup(text);
        });

        return dialog;
    }
    private CharacterBubbleData GetCharacterData(string dialogName)
    {
        foreach (var data in characterList)
        {
            if (data.dialogName == dialogName)
                return data;
        }

        return null;
    }


    IEnumerator DialogRoutine()
    {
        foreach (var line in dialogData.dialogList)
        {
            DialogUI dialog;
            Sequence animSeq;
            CharacterBubbleData characterData = GetCharacterData(line.Name);

            if (characterData == null)
            {
                Debug.LogError($"未找到角色配置: {line.Name}");
                continue;
            }

            dialog = CreateDialog(line.Speak, characterData, out animSeq);

            Wentin tool = GetComponent<Wentin>();

            if (characterData.characterObj != null)
            {
                tool.Start_And_Jump(characterData.characterObj);
            }

            tool = null;
            yield return animSeq.WaitForCompletion();//Dotween内置，等待动画完成

            // 步骤1：等待打字完成 或 玩家手动跳过
            yield return new WaitUntil(() =>   //WaitUntil的工作原理：会反复检查括号内的条件只有当条件返回 true 时，协程才会继续执行在条件返回 false 期间，协程会"暂停"在这一行
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    if (dialog.IsTyping())
                        dialog.CompleteLine();  // 立即完成文本
                }
                return !dialog.IsTyping();
            });
            //优化点：没说完就他们变下一句了
            yield return new WaitUntil(() =>
    Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)
);
            // 步骤2：等待玩家确认进入下一句
            // 等玩家确认下一句
            yield return new WaitUntil(() =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    return true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    {
                        return false;
                    }
                    return true;
                }

                return false;
            });
            Destroy(dialog.gameObject);
            yield return null;
            record.RecordLineNumberFunc(line.Num);
        }

        dialogFinished = true;//目前我想知道这个有什么用，但是先留着吧
        Debug.Log("对话完成");
    }
    //这个脚本用于重启剧情
    public void PlayNewPlot(string newPath)
    {
        StopAllCoroutines();        // 停掉旧剧情（非常关键）
        path = newPath;             // 更新路径
        LoadJson();                 // 重新加载 JSON
        StartCoroutine(DialogRoutine());  // 重新播放
    }
}


//说真的如果要管多个角色的可以一模一样的搞3个角色的代码和4个角色的代码
//角色动态多搞几个脚本用着，不和这个耦合
//如果一个角色有多个剧情的话，我不介意多搞几个预制件（R公司极好的克隆技术）

//关于特殊的角色动态决定再开一个脚本然后在这个脚本里面调用函数