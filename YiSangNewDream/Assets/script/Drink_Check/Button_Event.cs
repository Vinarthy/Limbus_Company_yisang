using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Event : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject all;
    public void OnButtonClick()
    {
        if (all != null)
        {
            // 将all游戏对象的激活状态设置为false
            all.SetActive(false);
        }
    }
}
