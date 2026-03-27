using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//思路：选中角色之后，后面的white变成可见状态，鼠标离开之后变成不可见
//搞了状态机之后就加几行代码的事情，先这么写
public class Choice : MonoBehaviour
{

    public GameObject Black;
    private bool blackWasActive;
    // Start is called before the first frame update
    void Start()
    {
        if (Black != null)
        {
            blackWasActive = Black.activeSelf;
        }
    }

    void OnMouseEnter()
    {
        // 当鼠标移入时，显示Black对象
        if (Black != null)
        {
            Black.SetActive(true);
        }
    }
    void OnMouseExit()
    {
        // 当鼠标移出时，隐藏Black对象
        if (Black != null)
        {
            Black.SetActive(false);
        }
    }
}
