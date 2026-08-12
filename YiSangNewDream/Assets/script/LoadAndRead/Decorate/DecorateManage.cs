using System;
using System.Collections.Generic;
using UnityEngine;

public class DecorateManage : MonoBehaviour
{
    [SerializeField] private Transform decorateParent;

    public List<DecorateItem> decorateList = new List<DecorateItem>();

    private readonly List<GameObject> spawnedDecorates = new List<GameObject>();
    private DecorateRAndL decorateRAndL;

    private void Awake()
    {
        decorateRAndL = new DecorateRAndL();
        Reload();
    }

    public void Reload()
    {
        ClearSpawnedDecorates();

        string decorateJson = decorateRAndL.LoadDecorate();
        DecorateWrapper wrapper = JsonUtility.FromJson<DecorateWrapper>("{\"Decorate\":" + decorateJson + "}");
        decorateList = wrapper != null && wrapper.Decorate != null
            ? wrapper.Decorate
            : new List<DecorateItem>();

        Transform parent = decorateParent != null ? decorateParent : transform;
        foreach (DecorateItem item in decorateList)
        {
            if (item.name == "Default" || item.path == "000")
                continue;

            GameObject prefab = Resources.Load<GameObject>(item.path);
            if (prefab == null)
            {
                Debug.LogError("Decorate prefab not found: " + item.path);
                continue;
            }

            spawnedDecorates.Add(Instantiate(prefab, parent));
        }
    }

    private void ClearSpawnedDecorates()
    {
        foreach (GameObject item in spawnedDecorates)
        {
            if (item != null)
                Destroy(item);
        }

        spawnedDecorates.Clear();
    }

    [Serializable]
    public class DecorateItem
    {
        public string name;
        public string _comment;
        public string path;
    }

    [Serializable]
    private class DecorateWrapper
    {
        public List<DecorateItem> Decorate;
    }
}