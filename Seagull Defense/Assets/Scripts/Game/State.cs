using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Build,
    Combat
};

public class State : GenericSingleton<State>
{
    [SerializeField]
    private UnityEvent buildStartEvent;
    [SerializeField]
    private UnityEvent buildEndEvent;
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
        Manager.Instance.RegisterWaveEndListener(EndCombatPhase);
        EnterBuildPhase();
        Data.Instance.UI.RegisterWaveButtonListener(EndBuildPhase);
    }

    //Phase transitions
    private void EnterBuildPhase()
    {
        state = GameState.Build;
        tilesRemaining = Data.Instance.MaxTiles;
        buildStartEvent.Invoke();
    }

    private void EndBuildPhase()
    {
        buildEndEvent.Invoke();
        Debug.Log("Ended build phase");
        EnterCombatPhase();
    }

    private void EnterCombatPhase()
    {
        state = GameState.Combat;
        Debug.Log("Entered combat phase!");
        combatStartEvent.Invoke();
    }

    private void EndCombatPhase()
    {
        combatEndEvent.Invoke();
        buildStartEvent.Invoke();
        wave++;
        EnterBuildPhase();
    }

    private void OnPathConstructed()
    {
        tilesRemaining--;
    }

    //Event listener registrations
    public void RegisterBuildStartListener(UnityAction action)
    {
        buildStartEvent.AddListener(action);
    }

    public void RegisterBuildEndListener(UnityAction action)
    {
        buildEndEvent.AddListener(action);
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
