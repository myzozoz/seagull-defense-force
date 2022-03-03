using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Data : GenericSingleton<Data>
{
    [SerializeField]
    private GameObject gridObject;
    [SerializeField]
    private GameObject tilemapObject;
    [SerializeField]
    private GameObject fogmapObject;
    [SerializeField]
    private GameObject hintmapObject;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private int fov;
    [SerializeField]
    private short maxTiles = 5;
    [SerializeField]
    private short firstSpawnRing = 5;
    [SerializeField]
    private short chunkSize = 10;
    [SerializeField]
    private BankSO bank;
    [SerializeField]
    private BankSO tileBank;

    private Grid grid;
    private HashSet<GameObject> iceCreams;
    private HashSet<GameObject> gullSpawns;
    

    private bool requireICRefresh = false;
    // Start is called before the first frame update
    void Start()
    {
        iceCreams = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Ice Cream"));
        State.Instance.RegisterCombatStartListener(UpdateSpawns);
    }

    void LateUpdate()
    {
        if (requireICRefresh)
        {
            iceCreams = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Ice Cream"));
            requireICRefresh = false;
            //Debug.Log("Ice creams updated");
        }
    }

    //public properties
    public GameObject GridObject
    {
        get { return gridObject; }
    }

    public GameObject TilemapObject
    {
        get { return tilemapObject; }
    }

    public GameObject FogmapObject
    {
        get { return fogmapObject; }
    }

    public GameObject HintmapObject
    {
        get { return hintmapObject; }
    }

    public UIController UI
    {
        get { return uiController; }
    }

    public Grid GridComponent
    {
        get { return gridObject.GetComponent<Grid>(); }
    }

    public MapController Map
    {
        get { return tilemapObject.GetComponent<MapController>(); }
    }

    public int FOV
    {
        get { return fov; }
    }

    public HashSet<GameObject> IceCreams
    {
        get { return iceCreams; }
    }

    public HashSet<GameObject> GullSpawns
    {
        get { return gullSpawns; }
    }

    public int ICCount
    {
        get { return iceCreams.Count; }
    }

    public void RefreshIceCreams()
    {
        requireICRefresh = true;
    }

    public short MaxTiles
    {
        get { return maxTiles; }
    }

    public short ChunkSize
    {
        get { return chunkSize; }
    }

    public BankSO Bank
    {
        get { return bank; }
    }

    public BankSO TileBank
    {
        get { return tileBank; }
    }


    //Public methods
    public int SpawnsInRing(int x)
    {
        return SpawnConfig.GetSeagullSpawnCount(x);
    }

    public short FirstSpawnRing
    {
        get { return firstSpawnRing; }
    }

    private void UpdateSpawns()
    {
        gullSpawns = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Gull Spawn"));
    }

}
