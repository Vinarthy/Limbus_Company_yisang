using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

// Tea / Season 的纯读写工具，不挂到 GameObject。
public class ReadAndLoadSeasonAndTea
{
    private readonly string path = Path.Combine(Application.persistentDataPath, "Other.json");

    [Serializable]
    public class MaterialItem
    {
        public int id;
        public string name;
        public bool unlocked;
    }

    [Serializable]
    private class TeaWrapper
    {
        public List<MaterialItem> Tea;
    }

    [Serializable]
    private class SeasonWrapper
    {
        public List<MaterialItem> Season;
    }

    public List<MaterialItem> LoadTea()
    {
        string arrayJson = LoadArray("Tea");
        TeaWrapper wrapper = JsonUtility.FromJson<TeaWrapper>("{\"Tea\":" + arrayJson + "}");
        return wrapper != null && wrapper.Tea != null ? wrapper.Tea : new List<MaterialItem>();
    }

    public List<MaterialItem> LoadSeason()
    {
        string arrayJson = LoadArray("Season");
        SeasonWrapper wrapper = JsonUtility.FromJson<SeasonWrapper>("{\"Season\":" + arrayJson + "}");
        return wrapper != null && wrapper.Season != null ? wrapper.Season : new List<MaterialItem>();
    }

    public void SaveTeaUnlocked(int id, bool unlocked)//保存茶叶的存档
    {
        SaveUnlocked("Tea", id, unlocked);
    }

    public void SaveSeasonUnlocked(int id, bool unlocked)//保存小料的存档（如果后续使用同一面板记得刷新一下）
    {
        SaveUnlocked("Season", id, unlocked);
    }

    private string LoadArray(string sectionName)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Other.json not found: " + path);
            return "[]";
        }

        string json = File.ReadAllText(path);
        int arrayStart = FindArrayStart(json, sectionName);
        int arrayEnd = FindArrayEnd(json, arrayStart);
        if (arrayStart < 0 || arrayEnd < 0)
        {
            Debug.LogError("Invalid " + sectionName + " data in Other.json.");
            return "[]";
        }

        return json.Substring(arrayStart, arrayEnd - arrayStart + 1);
    }

    private void SaveUnlocked(string sectionName, int id, bool unlocked)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Other.json not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        int arrayStart = FindArrayStart(json, sectionName);
        int arrayEnd = FindArrayEnd(json, arrayStart);
        if (arrayStart < 0 || arrayEnd < 0)
        {
            Debug.LogError("Invalid " + sectionName + " data in Other.json.");
            return;
        }

        string arrayJson = json.Substring(arrayStart, arrayEnd - arrayStart + 1);
        Match itemMatch = Regex.Match(arrayJson, "\\\"id\\\"\\s*:\\s*" + id + "(?!\\d)");
        if (!itemMatch.Success)
        {
            Debug.LogError("Missing " + sectionName + " id: " + id);
            return;
        }

        int itemEnd = arrayJson.IndexOf('}', itemMatch.Index);
        if (itemEnd < 0)
        {
            Debug.LogError("Invalid " + sectionName + " item: " + id);
            return;
        }

        string itemJson = arrayJson.Substring(itemMatch.Index, itemEnd - itemMatch.Index + 1);
        Match unlockedMatch = Regex.Match(itemJson, "\\\"unlocked\\\"\\s*:\\s*(true|false)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
        if (!unlockedMatch.Success)
        {
            Debug.LogError("Missing unlocked state for " + sectionName + " id: " + id);
            return;
        }

        int unlockedStart = itemMatch.Index + unlockedMatch.Index;
        string updatedArray = arrayJson.Substring(0, unlockedStart)
            + "\"unlocked\": " + unlocked.ToString().ToLower()
            + arrayJson.Substring(unlockedStart + unlockedMatch.Length);

        File.WriteAllText(path, json.Substring(0, arrayStart) + updatedArray + json.Substring(arrayEnd + 1));
    }

    private static int FindArrayStart(string json, string sectionName)
    {
        int sectionStart = json.IndexOf("\"" + sectionName + "\"", StringComparison.Ordinal);
        return sectionStart < 0 ? -1 : json.IndexOf('[', sectionStart);
    }

    private static int FindArrayEnd(string json, int arrayStart)
    {
        if (arrayStart < 0)
            return -1;

        int depth = 0;
        bool inString = false;
        for (int i = arrayStart; i < json.Length; i++)
        {
            char current = json[i];
            if (current == '"' && (i == 0 || json[i - 1] != '\\'))
                inString = !inString;

            if (inString)
                continue;

            if (current == '[')
                depth++;
            else if (current == ']' && --depth == 0)
                return i;
        }

        return -1;
    }
}