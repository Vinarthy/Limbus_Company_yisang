using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskControl : MonoBehaviour
{
    public string TaskPath = "Dialog/test"; // 建议给个默认值，路径不带扩展名
    public TextMeshProUGUI TargetText;

    void Start()
    {
        LoadJson();
    }

    [System.Serializable]
    public class TaskData
    {
        public string Text;
    }

    [System.Serializable]
    public class TaskWrapper
    {
        public TaskData[] Tasks;
    }

    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(TaskPath);

        if (jsonFile != null)
        {
            TaskWrapper wrapper = JsonUtility.FromJson<TaskWrapper>(jsonFile.text);

            if (wrapper != null && wrapper.Tasks != null && wrapper.Tasks.Length > 0)
            {
                if (TargetText != null)
                {
                    TargetText.text = wrapper.Tasks[0].Text;
                    Debug.Log($"加载成功：{TargetText.text}");
                }
                else
                {
                    Debug.LogWarning("TargetText 未赋值！");
                }
            }
            else
            {
                Debug.LogError("JSON 格式错误或 Tasks 数组为空！");
            }
        }
        else
        {
            Debug.LogError($"JSON 读取失败！请检查路径：Assets/Resources/{TaskPath}.json");
        }
    }
}
