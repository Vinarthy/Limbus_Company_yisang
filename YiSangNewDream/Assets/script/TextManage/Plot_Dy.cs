using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//附加：我认为这个函数应该是控制角色的，并调用上述的函数，不应该耦合
//再附加：这个脚本主要是剧情走向控制来着应该
public class Plot_Dy : MonoBehaviour
{
    // Start is called before the first frame update
    public Wentin wentin;
    public bool dialogFinished = false;
    private PlayerInfoList dialogData;//这个好像不用挂（
    private RecordLineNumber record;

    [Header("气泡坐标")]
    [SerializeField] private float x1;//其实可以直接改坐标然后在不管它然后在坐标处生成
    [SerializeField] private float y1;
    [SerializeField] private float z1;
    [SerializeField] private float x2;//其实可以直接改坐标然后在不管它然后在坐标处生成
    [SerializeField] private float y2;
    [SerializeField] private float z2;
    [SerializeField] private bool Reverse1=false;//如果是true则在实例化的时候把缩放的X*一个-1的事情
    [SerializeField] private bool Reverse2 = false;
    [Header("角色名字")]
    [SerializeField] private string L_CharacterName;
    [SerializeField] private string R_CharacterName;
    [Header("UI")]
    public GameObject DialogBox;

    [Header("角色")]
    public GameObject characterA;
    public GameObject characterB;//这个角色应该扫场景得到

    [Header("文件路径")]
    public string path;
    [Header("对话序号")]
    public int x=0;//默认为0，根据不同的对话序号引用不同的动作，直接改在MoveControl1里面
    void Start()
    {
        LoadJson();
        wentin.SceneMiddleStart();
        record = GetComponent<RecordLineNumber>();
        StartCoroutine(DialogRoutine());
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
    public DialogUI CreateDialogL(string text,out Sequence seq)//这个后面的代码没加进去，靠北
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject obj = Instantiate(DialogBox, canvas.transform);
        obj.transform.localPosition = new Vector3(x1, y1, z1);
        obj.transform.localScale = Vector3.one * 0.1f;



        DialogUI dialog = obj.GetComponent<DialogUI>();
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = obj.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        // DOTween 渐显动画
        seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.2f));                  // 淡入
        seq.Join(obj.transform.DOScale(new Vector3(1f, 0.5f, 1f), 0.35f).SetEase(Ease.OutBack)); // 待会改成下到上平移

        // 动画完全结束后再开始打字
        seq.OnComplete(() =>
        {
            dialog.Setup(text);  // 这里才是真正开始打字，其实还缺一个随着字的多少拓展漫画框长度的功能（
        });
        return dialog;
    }
    //还有一个问题是对话框有反转的情况。
    //噔噔咚（
    public DialogUI CreateDialogR(string text, out Sequence seq)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject obj = Instantiate(DialogBox, canvas.transform);

        // 重置本地位置和缩放
        obj.transform.localPosition = new Vector3(x2, y2, z2);
        obj.transform.localScale = Vector3.one * 0.1f;


        DialogUI dialog = obj.GetComponent<DialogUI>();
        //声明漫画框

        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = obj.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        // DOTween 渐显动画
        seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.35f));                  // 淡入
        seq.Join(obj.transform.DOScale(new Vector3(1f, 0.5f, 1f), 0.35f).SetEase(Ease.OutBack)); // 待会改成下到上平移

        seq.OnComplete(() =>
        {
            dialog.Setup(text); 
        });
        return dialog;
    }
    

    IEnumerator DialogRoutine()
    {
        foreach (var line in dialogData.dialogList)
        {
            DialogUI dialog;
            Sequence animSeq;
            record.RecordLineNumberFunc(line.Num);
            if (line.Name == L_CharacterName)
            {
                dialog = CreateDialogL(line.Speak,out animSeq);
                //这里加一个让角色有动态
                Wentin tool=GetComponent<Wentin>();
                tool.Start_And_Jump(characterA);
                tool = null;
            }
            else
            {
                dialog = CreateDialogR(line.Speak, out animSeq);//返回多个值：方法已经返回了DialogUI，但还需要返回Sequence明确意图：表明该参数纯粹用于输出，调用前不需要初始化
                Wentin tool = GetComponent<Wentin>();
                tool.Start_And_Jump(characterB);
                tool = null;
            }
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
                Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
            );
            Destroy(dialog.gameObject); 
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