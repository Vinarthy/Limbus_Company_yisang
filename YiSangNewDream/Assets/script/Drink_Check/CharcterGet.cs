using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterGet : MonoBehaviour
{
    //角色的检测逻辑，挂到角色身上和碰撞物检测，碰撞物就是tea
    //让数值传递到角色的身上
    //角色是触发器，茶杯是质量为0的刚体
    [Header("数值")]
    [SerializeField] int Bitter;
    [SerializeField] int Sour;
    [SerializeField] int Hot;
    [SerializeField] int Sweet;
    [SerializeField] int Thick;
    [SerializeField] int salty;
    [SerializeField] int fresh;
    [SerializeField] string[] names = new string[4];//目标名字
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag== "Drink") {
            finish_property_two Attribute = other.GetComponent<finish_property_two>();
            Bitter = Attribute.Bitter;
            Sour = Attribute.Sour;
            Hot = Attribute.Hot;
            Sweet = Attribute.Sweet;
            Thick = Attribute.Thick;
            salty = Attribute.salty;
            fresh = Attribute.fresh;
            names = (string[])Attribute.names.Clone();
        }
        else
        {
            return;
        }
    }
}
