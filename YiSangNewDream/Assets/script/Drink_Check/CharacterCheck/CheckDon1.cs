using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDon1 : MonoBehaviour
{
    [Header("设定检验数值")]
    [SerializeField] int lollymiaomiaomiaozZhanwei;
    [SerializeField] int Fresh;
    [Header("路径设定")]
    public string SuccessPath;
    public string FailurePath;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drink"))
        {
            // 玩家进入逻辑
            Debug.Log("开始判断");
            finish_property_two temp = other.GetComponent<finish_property_two>();
            lollymiaomiaomiaozZhanwei = temp.Sweet;
            Fresh = temp.fresh;
            Plot_Dy newdialog = GetComponent<Plot_Dy>();
            if (lollymiaomiaomiaozZhanwei >3&& Fresh>2)
            {
                newdialog.x = 1;
                newdialog.PlayNewPlot(SuccessPath);
                Debug.Log("更新幕");
                TimeBool.Instance.AdvanceType =
                    StoryAdvanceType.Scene;
            }
            else
            {
                newdialog.x = 2;
                newdialog.PlayNewPlot(FailurePath);
                Debug.Log("更新幕");
                TimeBool.Instance.AdvanceType =
                    StoryAdvanceType.Scene;
            }
            Destroy(other.gameObject);//这个没销毁掉啊贴图还在，到时候整一下
        }
    }
}
