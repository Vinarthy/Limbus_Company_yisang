using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGallary : MonoBehaviour
{
    //ÓÃÓÚ¿ªÍ¼¼ø
    public int id = 1;
    private void Start()
    {
        GallaryManager.Instance.UnlockGallaryItem(id);
    }
}
