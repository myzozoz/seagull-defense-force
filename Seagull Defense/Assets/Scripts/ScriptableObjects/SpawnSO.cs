using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "ScriptableObjects/Spawn")]
public class SpawnSO : ScriptableObject
{
    [SerializeField]
    public float baseSpawnInterval;
    [SerializeField]
    private int baseSpawnAmount;
    [SerializeField]
    private int spawnIncreaseAmount;
    [SerializeField]
    private float spawnIntervalLevelMultiplier;
    [SerializeField]
    public int spawnIncreaseInterval;

    public GameObject Spawnable;

    public int GetSpawnAmount(int level)
    {
        return baseSpawnAmount + (int)(level / spawnIncreaseInterval) * spawnIncreaseAmount;
    }

    public float GetSpawnInterval(int level)
    {
        return baseSpawnInterval * Mathf.Pow(spawnIntervalLevelMultiplier, Mathf.Floor(level / spawnIncreaseInterval));
    }
}
