using UnityEngine;

public class StartAndChooseCharacter : MonoBehaviour
{
    public GameObject TipPrefab;

    private GameObject tipInstance;

    void Start()
    {
        // 游戏开始时实例化 Tip 预制件
        tipInstance = Instantiate(TipPrefab);
    }

    void Update()
    {
        // 点击鼠标左键
        if (Input.GetMouseButtonDown(0) && tipInstance != null)
        {
            Destroy(tipInstance);
            tipInstance = null;
        }
    }
}