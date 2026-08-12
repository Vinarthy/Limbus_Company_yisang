using System.Collections.Generic;
using UnityEngine;

//�������ñ����������ȷ���ڼ����Ӧ�ĸ�Ԥ�Ƽ�
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

    void InitStoryData()//Resources�µ�·��
    {
        AddNode(
            1, 0, 1,
            "character/Chapter1Final/C1/S0/S1-2 Variant"
        );

        AddNode(
            1, 1, 1,
            "character/Chapter1Final/C1/S1/donqute"
        );

        AddNode(//����Ļ
            1, 1, 2,
            "character/Chapter1Final/C1/S2/ReCall Variant"
        );
        AddNode(//��2��
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
            1, 3, 4,
            "character/Chapter1Final/C3/S4/ReCall Variant"
);
        AddNode(
            1, 4, 1,
            "character/Chapter1Final/C4/S1/Rodio"
        );
        AddNode(//牛牛我会想你的
            1, 4, 2,
            "character/Chapter1Final/C4/S2/donqute Variant"
);
        AddNode(
            1, 4, 3,
            "character/Chapter1Final/C4/S3/Dorang1"
        );
        AddNode(
            1, 4, 4,
            "character/Chapter1Final/C4/S4/ReCall Variant"
        );
        AddNode(
            1,5,1,
            "character/Chapter1Final/C5/S1/Dongbai Variant"
            );
        AddNode(
            1, 6, 1,
            "character/Chapter1Final/C6/S1/Dongbai Variant"
            );
        AddNode(
            1, 6, 2,
            "character/Chapter1Final/C6/S2/donqute Variant Variant"
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

        Debug.LogError("�Ҳ�������ڵ�: " + key);

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
//�ı��أ�