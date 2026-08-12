using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sanfuUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    private ReadAndLoadSeasonAndTea sanfuR;

    void Awake()
    {
        sanfuR = new ReadAndLoadSeasonAndTea();
        sanfuR.SaveTeaUnlocked(4,true );
        sanfuR.SaveSeasonUnlocked(3, true);
    }

    // Update is called once per frame
}
//去把存档的那俩茶叶改成已解锁的