using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本负责管理“开始制作”按钮按下去的操作
public class Product : MonoBehaviour
{
    //1.切换摄像机角度
    //2.禁用下面的“出餐”按钮
    //3.禁用自身
    [Header("转换操作")]
    public GameObject controlobject;
    public GameObject FoodServing;
    private Camera_Move C_M;
    private void Start()
    {
        C_M = controlobject.GetComponent<Camera_Move>();
    }
    public void OnClick()
    {
        C_M.SecondAnimation();
        FoodServing.SetActive(false);
        gameObject.SetActive(false);
    }
}
