using UnityEngine;

public abstract class GalBase : MonoBehaviour
{
    public abstract void OnLine(int lineNum, PlayerInfo info);
}