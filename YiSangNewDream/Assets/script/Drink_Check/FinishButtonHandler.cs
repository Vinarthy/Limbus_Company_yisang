using UnityEngine;
using UnityEngine.UI;

public class FinishButtonHandler : MonoBehaviour
{
    public GameObject teaPrefab;

    private int Bitter, Sour, Hot, Sweet, Thick, salty, fresh;
    private string[] names;

    private GameObject sourceCup;
    private Camera_Move C_M;

    private void Start()
    {
        C_M = GetComponent<Camera_Move>();
    }
    public void ReceiveData(
        int Bitter, int Sour, int Hot, int Sweet,
        int Thick, int salty, int fresh,
        string[] names,
        GameObject cup
    )
    {
        this.Bitter = Bitter;
        this.Sour = Sour;
        this.Hot = Hot;
        this.Sweet = Sweet;
        this.Thick = Thick;
        this.salty = salty;
        this.fresh = fresh;
        this.names = (string[])names.Clone();
        this.sourceCup = cup;
    }

    public void OnClick()
    {
        GameObject newTea = Instantiate(teaPrefab);

        finish_property_two script = newTea.GetComponent<finish_property_two>();

        script.Bitter = Bitter;
        script.Sour = Sour;
        script.Hot = Hot;
        script.Sweet = Sweet;
        script.Thick = Thick;
        script.salty = salty;
        script.fresh = fresh;
        script.names = (string[])names.Clone();
        //这个地方再来个操控摄像机的
        C_M.FirstAnimation();
        Destroy(sourceCup); // 删除旧杯子
        gameObject.SetActive(false); // 按钮隐藏
    }
}
