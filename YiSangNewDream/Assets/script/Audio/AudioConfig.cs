using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [Serializable]
    public class AudioItem
    {
        public string id;          // ��ƵΨһ����
        public AudioClip clip;     // ��Ƶ��Դ
    }


    [Header("主背景音乐")]
    public List<AudioItem> bgmList = new List<AudioItem>();


    [Header("副背景音乐")]
    public List<AudioItem> secondaryBgmList = new List<AudioItem>();


    [Header("音效")]
    public List<AudioItem> sfxList = new List<AudioItem>();


    [Header("UI音效")]
    public List<AudioItem> uiList = new List<AudioItem>();


    //����IDѰ��BGM
    public AudioClip GetBGM(string id)
    {
        foreach (AudioItem item in bgmList)
        {
            if (item.id == id)
                return item.clip;
        }

        Debug.LogWarning("û���ҵ�BGM:" + id);

        return null;
    }


    //����IDѰ����Ч
    public AudioClip GetSecondaryBGM(string id)
    {
        foreach (AudioItem item in secondaryBgmList)
        {
            if (item.id == id)
                return item.clip;
        }

        Debug.LogWarning("没有找到副BGM:" + id);

        return null;
    }


    public AudioClip GetSFX(string id)
    {
        foreach (AudioItem item in sfxList)
        {
            if (item.id == id)
                return item.clip;
        }

        Debug.LogWarning("û���ҵ�SFX:" + id);

        return null;
    }


    //����IDѰ��UI����
    public AudioClip GetUI(string id)
    {
        foreach (AudioItem item in uiList)
        {
            if (item.id == id)
                return item.clip;
        }

        Debug.LogWarning("û���ҵ�UI��Ч:" + id);

        return null;
    }
}
