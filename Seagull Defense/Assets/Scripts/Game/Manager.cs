using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum Interaction
{
    //Left-click
    Primary,
    //Right-click
    Secondary
}

public class Manager : GenericSingleton<Manager>
{
    private MapController map;

    private int activeSpawns = 0;
    private UnityEvent waveEndEvent;

    void Start()
    {
        State.Instance.RegisterCombatStartListener(StartCombat);
        map = Data.Instance.Map;
        waveEndEvent = new UnityEvent();
        waveEndEvent.AddListener(() => Data.Instance.Bank.ChangeBalance(60));
    }

    void Update()
    {
        if (Data.Instance.ICCount <= 0)
        {
            SceneManager.LoadScene("SampleScene");
            Data.Instance.Bank.ResetBalance();
            Data.Instance.TileBank.ResetBalance();
        }
    }

    public void RegisterWaveEndListener(UnityAction action)
    {
        waveEndEvent.AddListener(action);
    }

    private void StartCombat()
    {
        int wave = State.Instance.Wave;
        Debug.Log($"Wave {wave} starts!");
        //Find relevant spawns
        //Spawn gulls
        List<GullSpawnController> spawns = new List<GullSpawnController>();
        foreach (GameObject spawn in Data.Instance.GullSpawns)
        {
            GullSpawnController gsc = spawn.GetComponent<GullSpawnController>();
            if (gsc != null && gsc.Active)
            {
                gsc.OnEndSpawn.AddListener(SpawnEndHandler);
                spawns.Add(gsc);
            }
        }
        activeSpawns = spawns.Count;
        StartCoroutine(WaitForSpawnEnd());
    }

    private IEnumerator WaitForSpawnEnd()
    {
        while (activeSpawns > 0)
        {
            yield return new WaitForSeconds(.1f);
        }

        Debug.Log("Wave ended I think");
        waveEndEvent.Invoke();
    }


    private void SpawnEndHandler()
    {
        activeSpawns--;
    }

    public void Interact(Interaction i, Vector3Int pos)
    {
        if (i == Interaction.Primary)
        {
            switch (Data.Instance.UI.Selected)
            {
                case SelectedButton.Path:
                    if (Data.Instance.TileBank.Balance > 0)
                        map.ConstructPath(pos);
                    break;
                case SelectedButton.Turret:
                    map.ConstructTurret(pos);
                    break;
            }
        }
    }
}
