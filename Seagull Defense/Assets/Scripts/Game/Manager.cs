using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : GenericSingleton<Manager>
{
    void Start()
    {
        State.Instance.RegisterCombatStartListener(StartCombat);
    }

    void Update()
    {
        if (Data.Instance.ICCount <= 0)
        {
            Debug.Log("Game lost shithead");
        }
    }

    private void StartCombat()
    {
        int wave = State.Instance.Wave;
        Debug.Log($"Wave {wave} starts!");
        //Find relevant spawns
        //Spawn gulls

    }
}
