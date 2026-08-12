using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SaveTotalJson : MonoBehaviour
{
    private const string DefaultSaveResourcePath = "DefaultOther";
    private static readonly string[] IdSections = { "Gallery", "Tea", "Season", "Shop" };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        EnsureSaveFile();
    }

    private void Awake()
    {
        EnsureSaveFile();
    }

    private static void EnsureSaveFile()
    {
        TextAsset defaultSave = Resources.Load<TextAsset>(DefaultSaveResourcePath);
        if (defaultSave == null)
        {
            Debug.LogError("Missing default save: Resources/" + DefaultSaveResourcePath + ".json");
            return;
        }

        string savePath = Path.Combine(Application.persistentDataPath, "Other.json");
        if (!File.Exists(savePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(savePath, defaultSave.text);
            return;
        }

        string playerSave = File.ReadAllText(savePath);
        string mergedSave = MergeMissingItems(playerSave, defaultSave.text);
        if (mergedSave != playerSave)
            File.WriteAllText(savePath, mergedSave);
    }

    private static string MergeMissingItems(string playerSave, string defaultSave)
    {
        string mergedSave = playerSave;
        foreach (string section in IdSections)
            mergedSave = MergeSectionById(mergedSave, defaultSave, section);

        return mergedSave;
    }

    private static string MergeSectionById(string playerSave, string defaultSave, string sectionName)
    {
        if (!TryGetArray(defaultSave, sectionName, out string defaultArray))
            return playerSave;

        if (!TryGetArray(playerSave, sectionName, out string playerArray, out int playerArrayStart, out int playerArrayEnd))
            return AddMissingSection(playerSave, sectionName, defaultArray);

        HashSet<int> playerIds = GetIds(playerArray);
        List<string> missingItems = new List<string>();
        foreach (string defaultItem in GetObjects(defaultArray))
        {
            if (TryGetId(defaultItem, out int id) && !playerIds.Contains(id))
                missingItems.Add(defaultItem);
        }

        if (missingItems.Count == 0)
            return playerSave;

        string playerContent = playerArray.Substring(1, playerArray.Length - 2).Trim();
        string separator = string.IsNullOrEmpty(playerContent) ? string.Empty : ",";
        string updatedArray = "[" + playerContent + separator + string.Join(",", missingItems) + "]";

        return playerSave.Substring(0, playerArrayStart) + updatedArray + playerSave.Substring(playerArrayEnd + 1);
    }

    private static string AddMissingSection(string json, string sectionName, string defaultArray)
    {
        int rootEnd = json.LastIndexOf('}');
        if (rootEnd < 0)
        {
            Debug.LogError("Other.json is invalid. Save migration was skipped.");
            return json;
        }

        string beforeRootEnd = json.Substring(0, rootEnd).TrimEnd();
        if (beforeRootEnd.EndsWith("{"))
            return beforeRootEnd + "\n  \"" + sectionName + "\": " + defaultArray + "\n}";

        return beforeRootEnd + ",\n  \"" + sectionName + "\": " + defaultArray + "\n}";
    }

    private static bool TryGetArray(string json, string sectionName, out string array)
    {
        return TryGetArray(json, sectionName, out array, out _, out _);
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

    private static HashSet<int> GetIds(string array)
    {
        HashSet<int> ids = new HashSet<int>();
        foreach (string item in GetObjects(array))
        {
            if (TryGetId(item, out int id))
                ids.Add(id);
        }
        return ids;
    }

    private static List<string> GetObjects(string array)
    {
        List<string> objects = new List<string>();
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] != '{')
                continue;

            int objectEnd = FindMatchingBracket(array, index, '{', '}');
            if (objectEnd < 0)
                break;

            objects.Add(array.Substring(index, objectEnd - index + 1));
            index = objectEnd;
        }
        return objects;
    }

    private static bool TryGetId(string itemJson, out int id)
    {
        Match match = Regex.Match(itemJson, "\\\"id\\\"\\s*:\\s*(\\d+)");
        return int.TryParse(match.Groups[1].Value, out id);
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