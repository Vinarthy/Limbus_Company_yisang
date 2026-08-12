using System;
using System.Collections.Generic;
using UnityEngine;

public class GallaryManager : MonoBehaviour
{
    public static GallaryManager Instance;

    public List<GallaryItem> galleryList;
    public event Action GalleryChanged;

    private GallaryLoadAndSave gallaryLoadAndSave;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gallaryLoadAndSave = new GallaryLoadAndSave();
        LoadGallery();
    }

    [Serializable]
    public class GallaryItem
    {
        public int id;
        public string name;
        public string description;
        public string path;
        public bool unlocked;
    }

    public GallaryItem GetGallaryItem(int id)
    {
        return galleryList == null ? null : galleryList.Find(item => item.id == id);
    }

    public bool UnlockGallaryItem(int id)//解锁图片的接口
    {
        GallaryItem item = GetGallaryItem(id);
        if (item == null || item.unlocked)
            return false;

        item.unlocked = true;
        gallaryLoadAndSave.SaveUnlocked(id, true);
        GalleryChanged?.Invoke();
        return true;
    }

    private void LoadGallery()
    {
        string galleryJson = gallaryLoadAndSave.LoadGallery();
        GallaryWrapper wrapper = JsonUtility.FromJson<GallaryWrapper>("{\"Gallery\":" + galleryJson + "}");
        galleryList = wrapper != null && wrapper.Gallery != null ? wrapper.Gallery : new List<GallaryItem>();
        galleryList.Sort((left, right) => left.id.CompareTo(right.id));
    }

    [Serializable]
    private class GallaryWrapper
    {
        public List<GallaryItem> Gallery;
    }
}
