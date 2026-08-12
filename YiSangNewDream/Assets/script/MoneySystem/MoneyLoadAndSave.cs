using System.IO;
using UnityEngine;

public class MoneyLoadAndSave
{
    private string path;

    public MoneyLoadAndSave()
    {
        path = Application.persistentDataPath + "/Other.json";
    }


    //读取金币
    public int LoadMoney()
    {
        string json = File.ReadAllText(path);

        int start = json.IndexOf("\"Money\":") + 8;

        int end = json.IndexOf(",", start);

        string value = json.Substring(start, end - start);

        return int.Parse(value);
    }


    //保存金币
    public void SaveMoney(int money)
    {
        string json = File.ReadAllText(path);


        int start = json.IndexOf("\"Money\":") + 8;

        int end = json.IndexOf(",", start);


        string newJson =
            json.Substring(0, start)
            + money
            + json.Substring(end);


        File.WriteAllText(path, newJson);
    }
}
//工具类，只负责存读