using System.IO;
using UnityEngine;

public class GallaryLoadAndSave
{
    private readonly string path = Path.Combine(Application.persistentDataPath, "Other.json");

    public string LoadGallery()
    {
        string json = File.ReadAllText(path);
        int start = json.IndexOf("\"Gallery\":");
        int end = start < 0 ? -1 : json.IndexOf("],", start);
        if (start < 0 || end < 0)
        {
            Debug.LogError("Other.json 中没有有效的 Gallery 数据。");
            return "[]";
        }

        return json.Substring(start + "\"Gallery\":".Length, end - start - "\"Gallery\":".Length + 1);
    }

    public void SaveUnlocked(int id, bool unlocked)
    {
        string json = File.ReadAllText(path);
        int galleryStart = json.IndexOf("\"Gallery\":");
        int galleryEnd = galleryStart < 0 ? -1 : json.IndexOf("],", galleryStart);
        if (galleryStart < 0 || galleryEnd < 0)
        {
            Debug.LogError("Other.json 中没有有效的 Gallery 数据。");
            return;
        }

        string gallery = json.Substring(galleryStart, galleryEnd - galleryStart);
        int itemStart = gallery.IndexOf("\"id\": " + id);
        int unlockedStart = itemStart < 0 ? -1 : gallery.IndexOf("\"unlocked\":", itemStart);
        if (unlockedStart < 0)
        {
            Debug.LogError("没有找到图鉴 id: " + id);
            return;
        }

        int valueStart = unlockedStart + "\"unlocked\":".Length;
        int valueEnd = gallery.IndexOf("}", valueStart);
        string updatedGallery = gallery.Substring(0, valueStart) + unlocked.ToString().ToLower() + gallery.Substring(valueEnd);
        File.WriteAllText(path, json.Substring(0, galleryStart) + updatedGallery + json.Substring(galleryEnd));
    }
}
