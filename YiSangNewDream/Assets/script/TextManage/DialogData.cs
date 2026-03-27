using System.Collections.Generic;

[System.Serializable]
public class PlayerInfo
{
    public string Name;   // 说话者名字
    public string Speak;  // 说的内容
    public int Num;       // 序号
}

[System.Serializable]
public class PlayerInfoList
{
    public List<PlayerInfo> dialogList;
}
