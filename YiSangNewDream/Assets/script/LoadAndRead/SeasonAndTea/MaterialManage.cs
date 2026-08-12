using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManage : MonoBehaviour
{
    [SerializeField] private Transform teaRoot;
    [SerializeField] private Transform seasonRoot;

    public List<ReadAndLoadSeasonAndTea.MaterialItem> teaList = new List<ReadAndLoadSeasonAndTea.MaterialItem>();
    public List<ReadAndLoadSeasonAndTea.MaterialItem> seasonList = new List<ReadAndLoadSeasonAndTea.MaterialItem>();

    private ReadAndLoadSeasonAndTea readAndLoad;

    private void Awake()
    {
        readAndLoad = new ReadAndLoadSeasonAndTea();
        Reload();
    }

    public void Reload()
    {
        teaList = readAndLoad.LoadTea();
        seasonList = readAndLoad.LoadSeason();
        ApplyUnlockedState(teaRoot, teaList);
        ApplyUnlockedState(seasonRoot, seasonList);
    }

    public bool UnlockTea(int id)
    {
        return Unlock(id, teaList, teaRoot, readAndLoad.SaveTeaUnlocked);
    }

    public bool UnlockSeason(int id)
    {
        return Unlock(id, seasonList, seasonRoot, readAndLoad.SaveSeasonUnlocked);
    }

    private static bool Unlock(int id, List<ReadAndLoadSeasonAndTea.MaterialItem> items, Transform root, Action<int, bool> save)
    {
        ReadAndLoadSeasonAndTea.MaterialItem item = items.Find(current => current.id == id);
        if (item == null || item.unlocked)
            return false;

        item.unlocked = true;
        save(id, true);
        ApplyUnlockedState(root, items);
        return true;
    }

    private static void ApplyUnlockedState(Transform root, List<ReadAndLoadSeasonAndTea.MaterialItem> items)//在list中决定SetActive的True或者False
    {
        if (root == null)
            return;

        foreach (ReadAndLoadSeasonAndTea.MaterialItem item in items)
        {
            int childIndex = item.id - 1;
            if (childIndex < 0 || childIndex >= root.childCount)
            {
                Debug.LogWarning(root.name + " is missing the child for id: " + item.id);
                continue;
            }

            root.GetChild(childIndex).gameObject.SetActive(item.unlocked);
        }
    }
}
//Start或者购买时触发ApplyUnlockedState函数
//即刷新视图
//加个if（如果在目标场景就ApplyUnlockedState）