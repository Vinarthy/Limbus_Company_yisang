using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BagReadAndLoad
{
    private readonly string savePath;

    public BagReadAndLoad()
    {
        savePath = Path.Combine(Application.persistentDataPath, "Other.json");
    }

    [Serializable]
    public class BagItem
    {
        public string Type;
        public string name;
        public string path;
        public int id;
        public string description;
    }

    [Serializable]
    private class BagWrapper
    {
        public List<BagItem> Bag;
    }

    public List<BagItem> LoadBag()
    {
        if (!File.Exists(savePath))
            return new List<BagItem>();

        string json = File.ReadAllText(savePath);
        if (!TryGetArray(json, "Bag", out string bagJson, out _, out _))
            return new List<BagItem>();

        BagWrapper wrapper = JsonUtility.FromJson<BagWrapper>("{\"Bag\":" + bagJson + "}");
        return wrapper != null && wrapper.Bag != null ? wrapper.Bag : new List<BagItem>();
    }

    public bool AddItem(string type, string itemName, string itemPath, string description)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Debug.LogError("Bag item name cannot be empty.");
            return false;
        }

        List<BagItem> items = LoadBag();
        foreach (BagItem existingItem in items)
        {
            if (existingItem.name == itemName)
                return false;
        }

        int nextId = 1;
        foreach (BagItem existingItem in items)
            nextId = Mathf.Max(nextId, existingItem.id + 1);

        BagItem newItem = new BagItem
        {
            Type = type,
            name = itemName,
            path = itemPath,
            id = nextId,
            description = description
        };

        return AppendItem(newItem);
    }

    private bool AppendItem(BagItem item)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("Other.json does not exist: " + savePath);
            return false;
        }

        string json = File.ReadAllText(savePath);
        string itemJson = JsonUtility.ToJson(item);
        if (TryGetArray(json, "Bag", out string bagJson, out int arrayStart, out int arrayEnd))
        {
            string content = bagJson.Substring(1, bagJson.Length - 2).Trim();
            string updatedArray = string.IsNullOrEmpty(content)
                ? "[" + itemJson + "]"
                : "[" + content + "," + itemJson + "]";

            File.WriteAllText(savePath, json.Substring(0, arrayStart) + updatedArray + json.Substring(arrayEnd + 1));
            return true;
        }

        int rootEnd = json.LastIndexOf('}');
        if (rootEnd < 0)
        {
            Debug.LogError("Other.json is invalid. Bag item was not saved.");
            return false;
        }

        string beforeRootEnd = json.Substring(0, rootEnd).TrimEnd();
        string separator = beforeRootEnd.EndsWith("{") ? string.Empty : ",";
        File.WriteAllText(savePath, beforeRootEnd + separator + "\n\"Bag\":[" + itemJson + "]\n}");
        return true;
    }

    private static bool TryGetArray(string json, string sectionName, out string array, out int arrayStart, out int arrayEnd)
    {
        array = null;
        arrayStart = -1;
        arrayEnd = -1;

        int sectionStart = json.IndexOf("\"" + sectionName + "\"", StringComparison.Ordinal);
        if (sectionStart < 0)
            return false;

        arrayStart = json.IndexOf('[', sectionStart);
        arrayEnd = FindMatchingBracket(json, arrayStart, '[', ']');
        if (arrayStart < 0 || arrayEnd < 0)
            return false;

        array = json.Substring(arrayStart, arrayEnd - arrayStart + 1);
        return true;
    }

    private static int FindMatchingBracket(string text, int start, char open, char close)
    {
        if (start < 0)
            return -1;

        int depth = 0;
        bool inString = false;
        for (int index = start; index < text.Length; index++)
        {
            char current = text[index];
            if (current == '"' && (index == 0 || text[index - 1] != '\\'))
                inString = !inString;
            if (inString)
                continue;
            if (current == open)
                depth++;
            else if (current == close && --depth == 0)
                return index;
        }

        return -1;
    }
}