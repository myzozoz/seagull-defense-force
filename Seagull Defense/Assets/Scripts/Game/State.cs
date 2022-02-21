using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Planning,
    Combat
};

public class State : GenericSingleton<State>
{
    [SerializeField]
    private UnityEvent combatStartEvent;

    private GameState state;
    private MapController map;
    private short tilesRemaining = 0;
    private int wave = 1;

    // Start is called before the first frame update
    void Start()
    {
        map = Data.Instance.Map;
        map.RegisterPathConstructListener(OnPathConstructed);
        EnterPlanningPhase();
    }

    private void EnterPlanningPhase()
    {
        state = GameState.Planning;
        tilesRemaining = Data.Instance.MaxTiles;
    }

    private void EnterCombatPhase()
    {
        state = GameState.Combat;
        combatStartEvent.Invoke();
    }

    private void OnPathConstructed()
    {
        tilesRemaining--;
        if (tilesRemaining == 0)
        {
            EnterCombatPhase();
        }
    }

    public void RegisterCombatStartListener(UnityAction action)
    {
        combatStartEvent.AddListener(action);
    }

    public int Wave
    {
        get { return wave; }
    }

    public GameState Current
    {
        get { return state; }
    }

    public short TilesRemaining
    {
        get { return tilesRemaining; }
    }
}
