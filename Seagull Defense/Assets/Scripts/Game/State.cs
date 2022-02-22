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
    private UnityEvent planningStartEvent;
    [SerializeField]
    private UnityEvent planningEndEvent;
    [SerializeField]
    private UnityEvent combatStartEvent;
    [SerializeField]
    private UnityEvent combatEndEvent;
    

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

    //Phase transitions
    private void EnterPlanningPhase()
    {
        state = GameState.Planning;
        tilesRemaining = Data.Instance.MaxTiles;
        planningStartEvent.Invoke();
    }

    private void EndPlanningPhase()
    {
        planningEndEvent.Invoke();
        Debug.Log("Ended planning phase");
    }

    private void EnterCombatPhase()
    {
        state = GameState.Combat;
        Debug.Log("Entered combat phase!");
        combatStartEvent.Invoke();
    }

    private void OnPathConstructed()
    {
        tilesRemaining--;
        if (tilesRemaining == 0)
        {
            EndPlanningPhase();
            EnterCombatPhase();
        }
    }

    //Event listener registrations
    public void RegisterPlanningStartListener(UnityAction action)
    {
        planningStartEvent.AddListener(action);
    }

    public void RegisterPlanningEndListener(UnityAction action)
    {
        planningEndEvent.AddListener(action);
    }

    public void RegisterCombatStartListener(UnityAction action)
    {
        Debug.Log($"Registering combat start listener {action}");
        combatStartEvent.AddListener(action);
    }

    public void RegisterCombatEndListener(UnityAction action)
    {
        combatEndEvent.AddListener(action);
    }

    //Public properties
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
