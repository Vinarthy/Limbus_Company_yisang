using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceCream : MonoBehaviour
{
    private ReadAndLoadSeasonAndTea icecream;

    void Awake()
    {
        icecream = new ReadAndLoadSeasonAndTea();
        icecream.SaveSeasonUnlocked(7, true);
    }
}
