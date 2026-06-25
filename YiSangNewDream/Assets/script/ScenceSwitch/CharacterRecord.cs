using System.Collections.Generic;
using UnityEngine;

public class CharacterRecord : MonoBehaviour
{
    [Header("要写入的角色预制件")]
    public List<GameObject> characterPrefabs = new List<GameObject>();

    private click clickScript;

    private void Start()
    {
        FindRecord();

        if (clickScript == null)
        {
            Debug.LogError("未找到持久化的 click 单例", this);
            return;
        }

        ClearCharacters();
        AddCharacters();
    }

    private void FindRecord()
    {
        // Record 会跨场景保留。不能使用 GameObject.Find，否则可能找到
        // 本帧即将被销毁的重复 Record，导致角色列表仍写在旧对象上。
        clickScript = click.Instance;
    }

    private void ClearCharacters()
    {
        clickScript.characters.Clear();
    }

    private void AddCharacters()
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab != null)
                clickScript.characters.Add(prefab);
        }
    }
}