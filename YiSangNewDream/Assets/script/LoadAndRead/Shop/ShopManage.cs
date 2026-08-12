using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;


    private ShopLoadAndSave shopLoadAndSave;

    private ReadAndLoadSeasonAndTea materialLoadAndSave;

    private BagReadAndLoad bagReadAndLoad;



    public List<ShopItem> shopList;

    public event Action ShopChanged;



    private void Awake()
    {

        if (Instance == null)
        {

            Instance = this;

            DontDestroyOnLoad(gameObject);


            shopLoadAndSave =
                new ShopLoadAndSave();

            materialLoadAndSave =
                new ReadAndLoadSeasonAndTea();

            bagReadAndLoad =
                new BagReadAndLoad();


            LoadShop();

        }
        else
        {
            Destroy(gameObject);
        }

    }



    [Serializable]
    public class ShopItem
    {

        public int id;

        public string name;

        public string description;

        public string path;

        public string Type;

        public string targetType;

        public int targetId;

        public bool purchased;

    }



    private void LoadShop()
    {

        string shopJson =
            shopLoadAndSave.LoadShop();



        shopList =
            JsonUtility.FromJson<ShopWrapper>(
                "{ \"Shop\":" + shopJson + "}"
            ).Shop;

    }




    [Serializable]
    private class ShopWrapper
    {
        public List<ShopItem> Shop;
    }





    public ShopItem GetShopItem(int id)
    {

        foreach (var item in shopList)
        {

            if (item.id == id)
                return item;

        }


        return null;

    }




    //这是购买脚本
    //接下来是扣钱和一个场景判断的if，如果场景是middle就要刷新一次材料区的界面，
    //购买脚本同时还得需要更新一下BagPack的（json里面）内容了，其他到没什么，判断是否加进去的话来个name匹配就可以
    public bool BuyShopItem(int id)
    {

        ShopItem item =
            GetShopItem(id);



        if (item == null)
            return false;



        if (item.purchased && id != 9)
            return false;

        if (id != 9)
        {
            item.purchased = true;
            shopLoadAndSave.SavePurchased(id, true);

            UnlockMaterial(item);
            bagReadAndLoad.AddItem(item.Type, item.name, item.path, item.description);
        }
        //哦莫这里没有改变材料区存档的逻辑，应该是shop再加个类型词条

        ShopChanged?.Invoke();

        
        //Ԥ��һ��ˢ���̵����Ϣͨ��
        //香飘飘热饮
        //下面开始场景材料刷新区
        if (SceneManager.GetActiveScene().name == "Middle")
        {
            GameObject baseMap = GameObject.Find("RealBaseMap");
            if (baseMap != null && baseMap.TryGetComponent(out MaterialManage materialManage))
            {
                materialManage.Reload();
            }
        }
        return true;

    }
    private void UnlockMaterial(ShopItem item)
    {
        if (item.Type != "Material")
            return;

        if (item.targetId <= 0)
        {
            Debug.LogError("Material shop item has invalid targetId: " + item.id);
            return;
        }

        if (item.targetType == "Tea")
        {
            materialLoadAndSave.SaveTeaUnlocked(item.targetId, true);
        }
        else if (item.targetType == "Season")
        {
            materialLoadAndSave.SaveSeasonUnlocked(item.targetId, true);
        }
        else
        {
            Debug.LogError("Material shop item has invalid targetType: " + item.targetType);
        }
    }
}