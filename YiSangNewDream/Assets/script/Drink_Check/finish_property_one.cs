using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

//这个脚本目前还缺少扫场景得到目标元素的功能
public class finish_property_one : MonoBehaviour
{
    // Start is called before the first frame update
    //这个绑定到杯子上，然后记录，判定都是这个代码负责
    private UnityEngine.UI.Button delivery;

    [SerializeField] private int Max_Tea_Num;
    [SerializeField] private int Max_Season_Num;
    private int tea_num = 0;
    private int season_num = 0;

    [Header("加料交互的目标位置")]
    public Vector3 TargetPosition;

    [Header("数值")]
    public int Bitter;
    public int Sour;
    public int Hot;
    public int Sweet;
    public int Thick;
    public int salty;
    public int fresh;
    public string[] names = new string[3];

    [Header("第二个茶")]
    public GameObject Tea2;//这个Tea2打算搞一个预制件，按了“出餐”那就生成预制件并传参出去

    [Header("堆点不知道怎么归类的东西")]
    private GameObject Botton_;
    private int i = 0;
    private Queue<GameObject> ingredientQueue = new Queue<GameObject>();
    private bool isProcessing = false;

    private void Start()
    {
        Bitter = 0; Sour = 0; Hot = 0; Sweet = 0; Thick = 0; salty = 0; fresh=0;//先初始化数值
        //功能：扫场景，扫到按钮就返回True，否则就false
        Botton_ = find_Target();

        Button btn = Botton_.GetComponent<Button>();
        btn.onClick.RemoveAllListeners(); // 防止重复注册
        Botton_.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("已触发");
        if (!ingredientQueue.Contains(other.gameObject))
            {
                ingredientQueue.Enqueue(other.gameObject);
            }

            TryProcessNext();
     }
    void TryProcessNext()//就是从你这开始的呃啊啊啊啊啊啊啊
    {
        if (isProcessing) return;
        if (ingredientQueue.Count == 0) return;
        GameObject next = ingredientQueue.Dequeue();
        StartCoroutine(ProcessIngredient(next));
    }

    IEnumerator ProcessIngredient(GameObject co_other)
    {
        isProcessing = true;

        CapsuleCollider2D capCollider = GetComponent<CapsuleCollider2D>();
        capCollider.enabled = false;

        if (co_other.CompareTag("Tea"))
        {
            tea_property Temp = co_other.GetComponent<tea_property>();

            if (tea_num < Max_Tea_Num)
            {
                yield return PlayPourAnimation(co_other.transform, Temp.originalposition);
                Bitter += Temp.Bitter;Sour += Temp.Sour;Hot += Temp.Hot;Sweet += Temp.Sweet;
                AddNameSafe(Temp.Name);
                tea_num++;
                passon();
            }
            else
            {
                co_other.transform.DOMove(Temp.originalposition, 0.3f);
            }
        }
        else if (co_other.CompareTag("Season"))
        {
            season_property Temp = co_other.GetComponent<season_property>();

            if (season_num < Max_Season_Num)
            {
                yield return PlayPourAnimation(co_other.transform, Temp.originalposition);
                 Sweet += Temp.Sweet;Thick += Temp.Thick;salty += Temp.salty;fresh += Temp.fresh;
                AddNameSafe(Temp.Name);//这里其实还差给子对象贴贴图，不过这个是以后的事情
                season_num++;
                passon();
            }
            else
            {
                co_other.transform.DOMove(Temp.originalposition, 0.3f);
            }
        }

        capCollider.enabled = true;
        isProcessing = false;

        TryProcessNext(); // 处理下一个
    }

    IEnumerator PlayPourAnimation(Transform t, Vector3 backPos)
    {
        Vector3 initialEulerAngles = t.eulerAngles;
        float swingDuration = 0.2f; // 每次摆动的时间
        int swingCount = 3; // 3次完整摆动

        for (int i = 0; i < swingCount * 2; i++) // *2 因为每次摆动包含去和回
        {
            float swingTimer = 0;
            float startAngle = (i % 2 == 0) ? -15f : 15f; // 偶数次从-15开始，奇数次从15开始
            float targetAngle = (i % 2 == 0) ? 15f : -15f; // 偶数次到15，奇数次到-15

            Quaternion startRot = Quaternion.Euler(initialEulerAngles.x, initialEulerAngles.y, startAngle);
            Quaternion targetRot = Quaternion.Euler(initialEulerAngles.x, initialEulerAngles.y, targetAngle);

            while (swingTimer < swingDuration)
            {
                swingTimer += Time.deltaTime;
                float tValue = swingTimer / swingDuration;
                t.rotation = Quaternion.Slerp(startRot, targetRot, tValue);
                yield return null;
            }
            t.rotation = targetRot; // 确保精确到达
        }
        float returnDuration = 0.35f;
        float returnTimer = 0;
        Vector3 returnStartPos = t.position;

        while (returnTimer < returnDuration)
        {
            returnTimer += Time.deltaTime;
            t.position = Vector3.Lerp(returnStartPos, backPos, returnTimer / returnDuration);
            yield return null;
        }
        t.position = backPos; 
        t.rotation = Quaternion.Euler(initialEulerAngles);
    }
    void AddNameSafe(string value)
    {
        for (int j = 0; j < names.Length; j++)
        {
            if (string.IsNullOrEmpty(names[j]))
            {
                names[j] = value;
                return;
            }
        }
    }


    GameObject find_Target()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas c in canvases)
        {
            if (c.name == "Canvas")   // 只认这个
            {
                Transform t = FindChildByName(c.transform, "Done");
                return t != null ? t.gameObject : null;
            }
        }

        return null;
    }
    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }


    bool Check_State()//这个函数用于检测是否符合出餐标准,按出餐按钮之后触发,如果出餐了就传参进预制件然后生成,然后把自己摧毁掉
    {
        if(tea_num >= Max_Tea_Num && season_num>= Max_Season_Num)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void passon()//现在的有一个小bug：就是要按出餐的按钮，但是现在是直接初始化上面的了，得先显示按钮，按了之后才初始化上面的杯子。
    {
        if (Check_State())
        {
            Botton_.SetActive(true);
            FinishButtonHandler handler = Botton_.GetComponent<FinishButtonHandler>();
            handler.ReceiveData(
                Bitter, Sour, Hot, Sweet,
                Thick, salty, fresh,
                names,
                this.gameObject
            );
        }
    }
    //private void OnButtonClicked()//移动摄像机，然后清除
    //{
    //    Camera_Move cameraMove = GetComponent<Camera_Move>();
    //    cameraMove.FirstAnimation();
    //    Destroy(gameObject);
    //}

}
//真要加个冰淇淋后期也是在这里面改改