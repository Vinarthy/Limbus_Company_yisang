using UnityEngine;
using UnityEngine.Rendering;

public class Close : MonoBehaviour
{
    public ShopUIAnimation shopAnimation;


    public void _Close()
    {
        if (shopAnimation != null)
        {
            AudioManager.Instance.PlayUI("UI3", 1);
            shopAnimation.CloseAnimation();
        }
    }
}