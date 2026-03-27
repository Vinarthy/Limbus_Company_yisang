using UnityEngine;
using DG.Tweening;
public class DragAndReturn2D : MonoBehaviour
{
    //拖拽代码
    //用鼠标拖动一个 2D 物体
    //松开鼠标时，物体会自动回到初始位置
    //如果拖动过程中碰到带有 Cup 标签的 2D 触发器对象，会停止拖拽状态
    private Vector3 originalPosition;
    public bool isDragging = false;

    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // 保持2D平面
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        if (isDragging != false)
        {
            transform.position = originalPosition;//回到原位置的动画

            transform.DOKill();

            // 从小到大
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.DOScale(originalScale, 0.1f)
                     .SetEase(Ease.OutBack);
            isDragging = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cup")
        {
            isDragging = false;
        }
    }
}
//现在是松开了如果碰到杯子的话那也就固定了，后面就是finish脚本的责任了其实