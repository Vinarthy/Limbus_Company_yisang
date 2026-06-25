using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTART : MonoBehaviour
{
    public Wentin wentin;

    public Vector3 startPos = new Vector3(-14.13f, -0.59f, 0f);
    public Vector3 endPos = new Vector3(1.01f, -0.59f, 5f);

    void Start()
    {
        wentin.MoveFromTo(startPos, endPos, 0.7f);
    }
}