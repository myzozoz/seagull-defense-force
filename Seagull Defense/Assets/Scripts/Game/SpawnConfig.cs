using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConfig: GenericSingleton<SpawnConfig>
{
    public static int GetSeagullSpawnCount(int x)
    {
        return (int)(x / 10) + 1;
    }
}
