using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentRoot : MonoBehaviour
{
    private static PersistentRoot instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
