using System.Collections.Generic;
using UnityEngine;

//剧情配置表，单纯查表，确定第几天对应哪个预制件
public class StoryDatabase : MonoBehaviour
{
    public static StoryDatabase Instance;

    private Dictionary<string, StoryNode> storyMap =
        new Dictionary<string, StoryNode>();

    private void Awake()
    {
        Instance = this;

        InitStoryData();
    }

    void InitStoryData()//Resources下的路径
    {
        AddNode(
            1, 1, 1,
            "StoryPrefabs/Chapter1/Day1/ClassroomMorning"
        );

        AddNode(
            1, 1, 2,
            "StoryPrefabs/Chapter1/Day1/StreetNight"
        );

        AddNode(
            1, 2, 1,
            "StoryPrefabs/Chapter1/Day2/HomeMorning"
        );
    }

    void AddNode(
        int chapter,
        int day,
        int scene,
        string prefabPath)
    {
        StoryNode node = new StoryNode();

        node.chapter = chapter;
        node.day = day;
        node.scene = scene;
        node.prefabPath = prefabPath;

        string key = GetKey(chapter, day, scene);

        storyMap.Add(key, node);
    }

    public StoryNode GetStoryNode(
        int chapter,
        int day,
        int scene)
    {
        string key = GetKey(chapter, day, scene);

        if (storyMap.ContainsKey(key))
        {
            return storyMap[key];
        }

        Debug.LogError("找不到剧情节点: " + key);

        return null;
    }

    string GetKey(
        int chapter,
        int day,
        int scene)
    {
        return $"{chapter}_{day}_{scene}";
    }
}
