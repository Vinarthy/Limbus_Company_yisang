using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyManage : MonoBehaviour
{
    // Start is called before the first frame update
    //1.制作经济系统，首先需要同步UI内容，由于不小心的屎山操作导致马内的显示框变成了两个，所以这里最好使用通信
    //2.首先是初始化，建议使用awake，挂在UI上，读取单例里面的内容
    //3.一开始是读取存档然后放在这个脚本的内容里面
    //4.后续增加购买之类的，购买的话购买逻辑->间接数据->存档
    public static MoneyManage Instance;
    public int CurrentMoney;
    private MoneyLoadAndSave moneySave;
    public System.Action<int> OnMoneyChanged;
    private void Awake()
    {
        //单例判断
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;//提供全局访问锚点

        //切换场景不销毁
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        //注册场景
    }
    void Start()
    {
        //初始化存档管理
        moneySave = new MoneyLoadAndSave();

        //读取存档同步金币
        CurrentMoney = moneySave.LoadMoney();

        Debug.Log("当前金币：" + CurrentMoney);
        OnMoneyChanged?.Invoke(CurrentMoney);
    }

    private void OnDestroy()
    {
        //取消注册，防止重复监听
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("金币脚本进入场景：" + scene.name);
        OnMoneyChanged?.Invoke(CurrentMoney);
    }
    public void AddMoney(int amount)
    {
        CurrentMoney += amount;

        moneySave.SaveMoney(CurrentMoney);


        //通知UI
        OnMoneyChanged?.Invoke(CurrentMoney);


        Debug.Log("增加金币：" + amount + " 当前金币：" + CurrentMoney);
    }

    public bool CostMoney(int amount)
    {
        if (CurrentMoney < amount)
        {
            Debug.Log("金币不足");
            return false;
        }


        CurrentMoney -= amount;


        moneySave.SaveMoney(CurrentMoney);


        //通知UI
        OnMoneyChanged?.Invoke(CurrentMoney);


        Debug.Log("消耗金币：" + amount + " 当前金币：" + CurrentMoney);


        return true;
    }

}
