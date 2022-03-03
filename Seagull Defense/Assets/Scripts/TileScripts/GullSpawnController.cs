using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GullSpawnController : MonoBehaviour
{
    [SerializeField]
    private SpawnSO config;
    private bool active = false;

    private UnityEvent endSpawnEvent;
    private int level;

    void Start()
    {
        if (endSpawnEvent == null)
            endSpawnEvent = new UnityEvent();
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
        level++;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        int spawned = 0;
        while (spawned < config.GetSpawnAmount(level))
        {
            GameObject.Instantiate(config.Spawnable, transform.position, Quaternion.identity);
            //Debug.Log("Seagull spawned! Prepare to die!");
            yield return new WaitForSeconds(config.GetSpawnInterval(level));
            spawned++;
        }

        endSpawnEvent.Invoke();
    }

    public bool Active
    {
        get { return active; }
    }

    public UnityEvent OnEndSpawn
    {
        get
        {
            if (endSpawnEvent == null)
                endSpawnEvent = new UnityEvent();
            return endSpawnEvent;
        }
    }
}
