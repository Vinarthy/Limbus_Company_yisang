using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class CallShop : MonoBehaviour
{
    //这个脚本负责把商店预制件实例化出来
    // Start is called before the first frame update
    public GameObject Prefab;
    public Transform ParentTransform;
    //按钮点击调用
    private GameObject currentShop;
    public void OpenShop()
    {
        //防止重复打开
        if (currentShop != null)
        {
            return;
        }


        currentShop = Instantiate(
            Prefab,
            ParentTransform
        );
        //来个音效喵
        AudioManager.Instance.PlayUI("UI2", 1);

        Debug.Log("商店打开");
    }
}
