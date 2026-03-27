using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTART : MonoBehaviour
{
    // Start is called before the first frame update
    public Wentin wentin;
    void Start()
    {
        Vector3 s = new Vector3(-14.13f, -0.59f, 0);
        Vector3 e = new Vector3(1.01f, -0.59f, 5);
        wentin.MoveFromTo(s, e, 0.7f);
    }

    // Update is called once per frame
}
