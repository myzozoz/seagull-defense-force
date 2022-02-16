using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : GenericSingleton<Manager>
{

    void Update()
    {
        if (Data.Instance.ICCount <= 0)
        {
            Debug.Log("Game lost shithead");
        }
    }
}
