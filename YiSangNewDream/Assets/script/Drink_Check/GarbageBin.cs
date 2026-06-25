using UnityEngine;

public class GarbageBin : MonoBehaviour
{
    [Header("销毁饮料后激活的物体")]
    public GameObject targetObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Drink"))
            return;

        // 激活目标物体
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }

        // 销毁饮料
        Destroy(other.gameObject);
    }
}
//回头加个小动画，移过来的时候垃圾桶开盖，鼠标松开的时候再销毁