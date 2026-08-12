using System.IO;
using UnityEngine;


public class ShopLoadAndSave
{

    private string path;


    public ShopLoadAndSave()
    {
        path = Application.persistentDataPath + "/Other.json";
    }



    //��ȡShop����
    public string LoadShop()
    {

        string json = File.ReadAllText(path);


        int start = json.IndexOf("\"Shop\":") + 7;


        int end = json.IndexOf("],", start) + 1;


        string shopJson =
            json.Substring(start, end - start);


        return shopJson;

    }





    //�޸���Ʒ����״̬
    public void SavePurchased(int id, bool purchased)
    {

        string json = File.ReadAllText(path);



        //�ҵ�Shop����
        int shopStart =
            json.IndexOf("\"Shop\":");



        int shopEnd =
            json.IndexOf("],", shopStart);



        string before =
            json.Substring(0, shopStart);



        string shop =
            json.Substring(shopStart, shopEnd - shopStart);



        string after =
            json.Substring(shopEnd);



        //��λ��Ʒid

        string idText =
            "\"id\": " + id;


        int itemStart =
            shop.IndexOf(idText);



        if (itemStart == -1)
        {
            Debug.LogError("û���ҵ���Ʒid:" + id);
            return;
        }



        //�ҵ�purchased

        int purchasedStart =
            shop.IndexOf("\"purchased\":", itemStart);



        int valueStart =
            purchasedStart + "\"purchased\":".Length;



        int valueEnd = valueStart;
        while (valueEnd < shop.Length && char.IsWhiteSpace(shop[valueEnd]))
            valueEnd++;
        while (valueEnd < shop.Length && char.IsLetter(shop[valueEnd]))
            valueEnd++;



        string newShop =
            shop.Substring(0, valueStart)
            + purchased.ToString().ToLower()
            + shop.Substring(valueEnd);



        string newJson =
            before
            + newShop
            + after;



        File.WriteAllText(path, newJson);

    }

}