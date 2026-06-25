using System.IO;
using UnityEngine;

//�������ܣ�һ����һ������������Ǵ��Ǽ�������

public static class SaveSystem//�̳��ھ�̬�࣬ȫ�ֽ�һ�ݣ�����ֱ�ӵ���
{
    private static string savePath =
        Application.persistentDataPath + "/save.json";

    // ����
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("�浵�ɹ�: " + savePath);
    }

    // ��ȡ
    public static SaveData LoadGame()//������Ϸ���洢��SaveData����
    {
        // û�д浵
        if (!File.Exists(savePath))
        {
            Debug.Log("�����ڴ浵������Ĭ�ϴ浵");

            SaveData defaultData = new SaveData();

            defaultData.chapter = 1;
            defaultData.day = 0;
            defaultData.scene = 1;

            SaveGame(defaultData);

            return defaultData;
        }

        string json = File.ReadAllText(savePath);

        SaveData data =
            JsonUtility.FromJson<SaveData>(json);

        Debug.Log("�����ɹ�");

        return data;
    }
}