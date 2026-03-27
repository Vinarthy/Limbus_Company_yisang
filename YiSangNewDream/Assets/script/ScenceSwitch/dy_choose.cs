using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dy_choose : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TargetPrefeb;
    public click c;
    //这个脚本写点击对应角色之后把上面这玩意的信息传输到click脚本里面
    //我说这玩意还能拉出来当个模块处有没有懂的
    void OnMouseDown()
    {
        Debug.Log("鼠标按下了思密达 ");
        //后面是令c的character=TargetPrefeb
        if (c != null && TargetPrefeb != null)
        {
            click.Instance.character = TargetPrefeb;
        }
        else
        {
            Debug.LogWarning("click实例或目标预制件未分配!");
            return;
        }
        string targetScene = "Middle";
        SceneManager.LoadScene(targetScene);
    }
}
