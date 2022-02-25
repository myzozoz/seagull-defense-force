using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "ScriptableObjects/Spawn")]
public class SpawnSO : ScriptableObject
{
    public int level;
    public float spawnInterval;
    public int spawnAmount;
    public GameObject spawnable;

    public int Level
    {
        get { return level; }
    }

    public float SpawnInterval
    {
        get { return spawnInterval; }
    }

    public float SpawnAmount
    {
        get { return spawnAmount; }
    }

    public GameObject Spawnable
    {
        get { return spawnable; }
    }
}
