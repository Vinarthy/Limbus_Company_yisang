using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class season_property : MonoBehaviour
{
    //这个脚本管小料的信息
    public string Name;
    public int Sweet;
    public int Thick;
    public int salty;
    public int fresh;
    public Sprite sprite;//负责小料，但是我懒得搞这个了目前
    public Vector3 originalposition;
    void Start()
    {
        originalposition = transform.position;//设置实例化位置
    }
}
