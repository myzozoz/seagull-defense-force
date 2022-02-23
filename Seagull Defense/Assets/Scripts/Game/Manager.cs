using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        State.Instance.RegisterCombatStartListener(StartCombat);
        map = Data.Instance.Map;
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

    public void Interact(Interaction i, Vector3Int pos)
    {
        if (i == Interaction.Primary)
        {
            switch (Data.Instance.UI.Selected)
            {
                case SelectedButton.Path:
                    map.ConstructPath(pos);
                    break;
                case SelectedButton.Turret:
                    map.ConstructTurret(pos);
                    break;
            }
        }
    }
}
