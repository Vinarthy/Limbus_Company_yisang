using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//这个脚本用来管理检查的逻辑，具体是和标准数值相比对
//然后管理顾客各种各样的反应
public class check1 : MonoBehaviour
{
    [Header("设定检验数值")]
    [SerializeField] int lollymiaomiaomiaozZhanwei;
    [Header("路径设定")]
    public string SuccessPath;
    public string FailurePath;
    //后面是不同的判定操作，我一会写，备注：杯子的标签是Drink。
    //可以再次Plot_Dy脚本然后说不同的话
    //1.碰撞然后判定数值是否合规
    //2.然后根据数值有没有到然后加载SuccessPath和FailurePath再搞新的moveControl然后重启脚本。
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drink"))
        {
            // 玩家进入逻辑
            Debug.Log("开始判断");
            finish_property_two temp=other.GetComponent<finish_property_two>();
            lollymiaomiaomiaozZhanwei = temp.Sweet;
            Plot_Dy newdialog = GetComponent<Plot_Dy>();
            if (lollymiaomiaomiaozZhanwei >= 1)
            {
                newdialog.x = 1;
                newdialog.PlayNewPlot(SuccessPath);
            }
            else
            {
                newdialog.x = 2;
                newdialog.PlayNewPlot(FailurePath);
            }
            Destroy(other.gameObject);//这个没销毁掉啊贴图还在，到时候整一下
        }
    }
}
//那如何分别管理不同的对话段落的动作呢（？
