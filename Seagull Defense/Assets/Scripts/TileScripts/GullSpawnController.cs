using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullSpawnController : MonoBehaviour
{
    private SpawnConfigData config;
    private bool active = false;

    void Start()
    {
        config = SpawnConfig.Instance.Default;
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Seagull sg = collider.gameObject.GetComponent<Seagull>();
        if (sg != null)
        {
            //Debug.Log("Seagull just entered the spawnosphere");
            sg.OnSpawnEnter();
        }
    }

    public void Activate()
    {
        if (active) return;
        active = true;
        State.Instance.RegisterCombatStartListener(StartSpawning);
    }

    public void Deactivate()
    {
        //State should offer API to deregister listeners
        active = false;
    }

    public void StartSpawning()
    {
        if (!active) return;
        Debug.Log("Starting spawning");
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        int spawned = 0;
        Debug.Log("Spawn routine started");
        Debug.Log($"Spawn config | spawn amount: {config.SpawnAmount}");
        if (config.SpawnAmount == 0)
        {
            config = SpawnConfig.Instance.Default;
        }

        while (spawned < config.SpawnAmount)
        {
            GameObject.Instantiate(config.Spawnable, transform.position, Quaternion.identity);
            //Debug.Log("Seagull spawned! Prepare to die!");
            yield return new WaitForSeconds(config.SpawnInterval);
            spawned++;
        }
    }
}
