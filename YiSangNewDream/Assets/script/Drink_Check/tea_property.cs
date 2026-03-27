using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tea_property : MonoBehaviour
{
    public string Name;//管理茶的名称
    public int Bitter;
    public int Sour;
    public int Hot;
    public int Sweet;
    public Sprite sprite;//这个是贴图，为了制作的时候放杯子里面

    public Vector3 originalposition;
    void Start()
    {
        originalposition = transform.position;
    }
}
