using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskControl : MonoBehaviour
{
    public string TaskPath = "Dialog/test"; // �������Ĭ��ֵ��·��������չ��
    private TextMeshProUGUI TargetText;

    void Start()
    {
        FindTargetText();
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
                    Debug.Log($"���سɹ���{TargetText.text}");
                }
                else
                {
                    Debug.LogWarning("TargetText δ��ֵ��");
                }
            }
            else
            {
                Debug.LogError("JSON ��ʽ����� Tasks ����Ϊ�գ�");
            }
        }
        else
        {
            Debug.LogError($"JSON ��ȡʧ�ܣ�����·����Assets/Resources/{TaskPath}.json");
        }
    }
    public void SetTaskPath(string taskPath)
    {
        if (string.IsNullOrEmpty(taskPath))
            return;

        TaskPath = taskPath;

        if (TargetText == null)
            FindTargetText();

        LoadJson();
    }
    private void FindTargetText()
    {
        GameObject canvasObj = GameObject.Find("Canvas");

        if (canvasObj == null)
        {
            Debug.LogError("δ�ҵ� Canvas");
            return;
        }

        Transform taskTransform =
            canvasObj.transform
            .Find("Task/tasknote/task");

        if (taskTransform == null)
        {
            Debug.LogError("δ�ҵ�·����Canvas/Task/tasknote/task");
            return;
        }

        TargetText = taskTransform.GetComponent<TextMeshProUGUI>();

        if (TargetText == null)
        {
            Debug.LogError("task ��û�� TextMeshProUGUI ���");
        }
    }
}
