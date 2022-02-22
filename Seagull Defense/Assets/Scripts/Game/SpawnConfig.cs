using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnConfigData
{
    public int Level;
    public float SpawnInterval;
    public int SpawnAmount;
    public GameObject Spawnable;
}

public class SpawnConfig: GenericSingleton<SpawnConfig>
{
    [SerializeField]
    private SpawnConfigData defaultSpawnConfig;

    public static int GetSeagullSpawnCount(int x)
    {
        return (int)(x / 10) + 1;
    }

    public SpawnConfigData Default
    {
        get { return defaultSpawnConfig; }
    }
}
