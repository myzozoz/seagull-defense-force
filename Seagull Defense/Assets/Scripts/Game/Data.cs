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
    private int fov;
    [SerializeField]
    private short maxTiles = 5;
    [SerializeField]
    private short firstSpawnRing = 5;
    [SerializeField]
    private short chunkSize = 10;

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
            Debug.Log("Ice creams updated");
        }
    }

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

    public Grid GridComponent
    {
        get { return gridObject.GetComponent<Grid>(); }
    }

    public MapController Map
    {
        get { return tilemapObject.GetComponent<MapController>(); ; }
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
