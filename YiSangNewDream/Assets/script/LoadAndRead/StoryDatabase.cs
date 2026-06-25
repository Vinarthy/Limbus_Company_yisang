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
            1, 0, 1,
            "character/Chapter1Final/C1/S0/S1-2 Variant"//这个要在别的场景生成而且特么要指定父物体，开场生成之后就继续了
        );

        AddNode(
            1, 1, 1,
            "character/Chapter1Final/C1/S1/donqute"
        );

        AddNode(//第三幕
            1, 1, 2,
            "character/Chapter1Final/C1/S2/ReCall Variant"
        );
        AddNode(//第2天
            1, 2, 1,
            "character/Chapter1Final/C2/S1/FaustTouming"
        );
        AddNode(
            1, 2, 2,
            "character/Chapter1Final/C2/S2/donqute"
        );
        AddNode(
            1, 2, 3,
            "character/Chapter1Final/C2/S3/Shrenne"
        );
        AddNode(
            1, 2, 4,
            "character/Chapter1Final/C2/S4/ReCall"
        );
        AddNode(
            1, 3, 1,
            "character/Chapter1Final/C3/S1/Faust"
        );
        AddNode(
            1, 3, 2,
            "character/Chapter1Final/C3/S2/honglu"
        );
        AddNode(
            1, 3, 3,
            "character/Chapter1Final/C3/S3/Dongbai"
        );
        AddNode(
            1, 4, 1,
            "character/Chapter1Final/C4/S1/Rodio"
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
//改变呢？