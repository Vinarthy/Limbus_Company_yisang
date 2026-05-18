using System.Collections.Generic;
using UnityEngine;

public class CharacterRecord : MonoBehaviour
{
    [Header("要写入的角色预制件")]
    public List<GameObject> characterPrefabs = new List<GameObject>();

    private click clickScript;

    void Start()
    {
        FindRecord();

        if (clickScript == null)
        {
            Debug.LogError("未找到 Record 上的 click 脚本");
            return;
        }

        ClearCharacters();

        AddCharacters();
    }

    // 查找 Record 上的 click 脚本
    void FindRecord()
    {
        GameObject recordObj = GameObject.Find("Record");

        if (recordObj != null)
        {
            clickScript = recordObj.GetComponent<click>();
        }
    }

    // 清空 characters 列表
    void ClearCharacters()
    {
        clickScript.characters.Clear();
    }

    // 添加新的 prefab
    void AddCharacters()
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab == null)
                continue;

            clickScript.characters.Add(prefab);
        }
    }
}