using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DecorateRAndL
{
    private readonly string savePath = Path.Combine(Application.persistentDataPath, "Other.json");

    public string LoadDecorate()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("Other.json not found: " + savePath);
            return "[]";
        }

        string json = File.ReadAllText(savePath);
        int sectionStart = json.IndexOf("\"Decorate\"", StringComparison.Ordinal);
        int arrayStart = sectionStart < 0 ? -1 : json.IndexOf('[', sectionStart);
        int arrayEnd = FindArrayEnd(json, arrayStart);
        if (arrayStart < 0 || arrayEnd < 0)
        {
            Debug.LogError("Invalid Decorate data in Other.json.");
            return "[]";
        }

        return json.Substring(arrayStart, arrayEnd - arrayStart + 1);
    }

    public DecorateManage.DecorateItem GetCurrentDecorate()
    {
        DecorateWrapper wrapper = JsonUtility.FromJson<DecorateWrapper>("{\"Decorate\":" + LoadDecorate() + "}");
        if (wrapper != null && wrapper.Decorate != null && wrapper.Decorate.Count > 0)
            return wrapper.Decorate[0];

        return new DecorateManage.DecorateItem { name = "Default", path = "000" };
    }

    public bool SaveDecorate(string name, string path)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("Other.json not found: " + savePath);
            return false;
        }

        string json = File.ReadAllText(savePath);
        int sectionStart = json.IndexOf("\"Decorate\"", StringComparison.Ordinal);
        int arrayStart = sectionStart < 0 ? -1 : json.IndexOf('[', sectionStart);
        int arrayEnd = FindArrayEnd(json, arrayStart);
        if (arrayStart < 0 || arrayEnd < 0)
        {
            Debug.LogError("Invalid Decorate data in Other.json.");
            return false;
        }

        DecorateManage.DecorateItem item = new DecorateManage.DecorateItem
        {
            name = name,
            _comment = "装饰品内容",
            path = path
        };
        string array = "[" + JsonUtility.ToJson(item) + "]";
        File.WriteAllText(savePath, json.Substring(0, arrayStart) + array + json.Substring(arrayEnd + 1));
        return true;
    }

    private static int FindArrayEnd(string json, int arrayStart)
    {
        if (arrayStart < 0)
            return -1;

        int depth = 0;
        bool inString = false;
        for (int index = arrayStart; index < json.Length; index++)
        {
            char current = json[index];
            if (current == '"' && (index == 0 || json[index - 1] != '\\'))
                inString = !inString;

            if (inString)
                continue;

            if (current == '[')
                depth++;
            else if (current == ']' && --depth == 0)
                return index;
        }

        return -1;
    }

    [Serializable]
    private class DecorateWrapper
    {
        public List<DecorateManage.DecorateItem> Decorate;
    }
}