using System.IO;
using UnityEngine;

//俩个功能，一个存一个读，存读都是纯那几个数据
public static class SaveSystem
{
    private static string savePath =
        Application.persistentDataPath + "/save.json";

    // 保存
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("存档成功: " + savePath);
    }

    // 读取
    public static SaveData LoadGame()//加载游戏并存储在SaveData里面
    {
        // 没有存档
        if (!File.Exists(savePath))
        {
            Debug.Log("不存在存档，创建默认存档");

            SaveData defaultData = new SaveData();

            defaultData.chapter = 1;
            defaultData.day = 1;
            defaultData.scene = 1;

            SaveGame(defaultData);

            return defaultData;
        }

        string json = File.ReadAllText(savePath);

        SaveData data =
            JsonUtility.FromJson<SaveData>(json);

        Debug.Log("读档成功");

        return data;
    }
}