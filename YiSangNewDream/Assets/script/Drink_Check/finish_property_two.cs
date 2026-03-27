using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish_property_two : MonoBehaviour
{
    // Start is called before the first frame update
    //这个管的是第二个茶，管和角色碰撞的，我先懒得写这个，我先写别的嘻嘻嘻
    //按出餐的话这个就
    [Header("数值")]
    public int Bitter;
    public int Sour;
    public int Hot;
    public int Sweet;
    public int Thick;
    public int salty;
    public int fresh;
    public string[] names = new string[4];
}
