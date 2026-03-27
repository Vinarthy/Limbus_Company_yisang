using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//该板块就一个代码：管移动的
//没有update之类的，全靠外部启动
public class Wentin : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //if (gameObject != null)
    //{
    //    Vector3 startPoint = new Vector3(-12.13f,-0.59f,0);
    //    Vector3 endPoint = new Vector3(1.01f, -0.59f, 5);
    //    // 使用 DOMove 从起始点移动到终点
    //    transform.position = startPoint;
    //    transform.DOMove(endPoint, 1f).SetEase(Ease.Linear);
    //}
    //else
    //{
    //    Debug.LogError("myGameObject 未赋值");
    //}
            public void MoveFromTo(Vector3 startPos, Vector3 endPos, float duration)
            {
        transform.position = startPos;
        transform.DOMove(endPos, duration).SetEase(Ease.OutQuad);
            }


    public void Start_And_Jump( GameObject character)//把character放进去
    {
        Vector3 originalScale = character.transform.localScale;
        float originalY = originalScale.y;

        // 2. 将y值放大一点点
        float targetScaleY = originalY * 1.1f;
        Tweener scaleUpTween = character.transform.DOScaleY(targetScaleY, 0.1f)//前声明变量储存一个补间动画
            .OnComplete(() =>
            {
                // 3. 回到原来的y值
                character.transform.DOScaleY(originalY, 0.1f);
            });
    }

    public void SceneMiddleStart()//开始的时候用一下，目的是达到“弹起来的效果”
    {
        Vector3 originalScale = transform.localScale;
        float originalY = originalScale.y;
        transform.localScale = new Vector3(originalScale.x, 0f, originalScale.z);
        transform.DOScale(originalScale, 0.1f);
    }
}


