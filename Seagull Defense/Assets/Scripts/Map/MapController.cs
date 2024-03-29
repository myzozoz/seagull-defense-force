using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class MapController : MonoBehaviour
{
    [SerializeField]
    private Tile pathTile;
    [SerializeField]
    private Tile grassTile;
    [SerializeField]
    private Tile dirtTile;
    [SerializeField]
    private Tile spawnTile;
    [SerializeField]
    private Tile spawnHintTile;
    [SerializeField]
    private Tile turretTile;
    [SerializeField]
    private TowerSO turretSO;
    [SerializeField]
    private UnityEvent pathConstructedEvent;

    private Tilemap tilemap;
    private Tilemap fogmap;
    private Tilemap hintMap;



    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        fogmap = Data.Instance.FogmapObject.GetComponent<Tilemap>();
        hintMap = Data.Instance.HintmapObject.GetComponent<Tilemap>();
        State.Instance.RegisterBuildEndListener(GenerateRing);
    }

    void Update()
    {
        //Debug.Log($"This is my tilemap: {tilemap}");
    }

    public void ConstructPath(Vector3Int c)
    {
        //If tile is "path buildable" and there is an adjacent pathtile
        if (tilemap.HasTile(c)
            && (tilemap.GetTile(c).name == grassTile.name || (State.Instance.Current == GameState.Build && tilemap.GetTile(c).name == spawnTile.name))
            && NeighborsContain(c, new List<Tile>() { pathTile, turretTile })
            && Data.Instance.TileBank.ChangeBalance(-1))
        {
            //Set tile
            tilemap.SetTile(c, pathTile);
            //Convert nearby fog tiles to grass tiles.
            UpdateVision(c);
            //Invoke listening events
            pathConstructedEvent.Invoke();
        }
    }

    public void ConstructTurret(Vector3Int c)
    {
        if (tilemap.HasTile(c) && tilemap.GetTile(c).name == pathTile.name && Data.Instance.Bank.ChangeBalance(-turretSO.cost))
        {
            tilemap.SetTile(c, turretTile);
        }
    }

    public void SelectCell(Vector3Int c)
    {
        //Debug.Log($"Grid coordinates: {c} | Cube coordinates: {Hexer.GridToCube(c)} | Distance to origin: {Hexer.Distance(new Vector2Int(0,0), c)} | Tile: {tilemap.GetTile(new Vector3Int(c.x, c.y, 0)).name}");
    }

    private bool NeighborsContain(Vector3Int pos, List<Tile> tiles)
    {
        List<Vector3Int> nList = Hexer.GetNeighbors(pos);
        HashSet<string> tileNames = new HashSet<string>();
        foreach (Tile t in tiles)
        {
            tileNames.Add(t.name);
        }

        foreach (Vector3Int n in nList)
        {
            if (tileNames.Contains(tilemap.GetTile(n).name))
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateVision(Vector3Int c)
    {
        List<Vector3Int> coordinates = Hexer.GetCoordinatesInRange(c, Data.Instance.FOV);
        foreach (Vector3Int a in coordinates)
        {
            // Check if we need to generate more of the map
            if (!tilemap.HasTile(a))
            {
                GenerateChunk(a);
            }
            
            if (fogmap.HasTile(a))
            {
                fogmap.SetTile(a, null);
            }

            if (hintMap.HasTile(a))
            {
                hintMap.SetTile(a, null);
            }

            if (tilemap.GetTile(a).name == spawnTile.name)
            {
                ActivateSpawn(a);
            }
        }
    }

    private void ActivateSpawn(Vector3Int a)
    {
        if (fogmap.HasTile(a))
        {
            hintMap.SetTile(a, spawnHintTile);
        }

        GullSpawnController spawn = tilemap.GetInstantiatedObject(a).GetComponent<GullSpawnController>();
        if (spawn != null)
        {
            spawn.Activate();
        }
    }

    private void ActivateRing(int d)
    {
        foreach (Vector3Int v in Hexer.Ring(Vector3Int.zero, d))
        {
            if (tilemap.HasTile(v) && tilemap.GetTile(v).name == spawnTile.name)
            {
                ActivateSpawn(v);
            }
        }
    }

    public void RegisterPathConstructListener(UnityAction action)
    {
        pathConstructedEvent.AddListener(action);
    }

    public void GenerateRing()
    {
        int d = State.Instance.Wave + Data.Instance.FirstSpawnRing;
        int spawnCount = Data.Instance.SpawnsInRing(d);
        int ringSize = Hexer.RingSize(d);
        List<Vector3Int> ring = Hexer.Ring(Vector3Int.zero, d);
        List<Vector3Int> unplaced = new List<Vector3Int>();
        int known = 0;
        int knownSpawns = 0;
        //Debug.Log("Generating new ring!");
        foreach (Vector3Int v in ring)
        {
            if (tilemap.HasTile(v))
            {
                if (tilemap.GetTile(v).name == spawnTile.name)
                {
                    knownSpawns++;
                }
                known++;
            }
            else
            {
                unplaced.Add(v);
            }
        }
        
        foreach (Vector3Int v in unplaced)
        {
            //Debug.Log($"Placing {v} | Spawns in ring {spawnCount} | known spawns: {knownSpawns} | Ring size: {ringSize} | Known tiles: {known}");
            float spawnProb = known < Hexer.RingSize(d) ? (float)(spawnCount - knownSpawns) / (float)(ringSize - known) : 0;
            //Debug.Log($"Spawn prob at d={d}: {spawnProb}");
            if (Random.Range(0f, 1f) < spawnProb)
            {
                tilemap.SetTile(v, spawnTile);
                known++;
                knownSpawns++;
            }
            else
            {
                //tilemap.SetTile(v, grassTile);
                known++;
            }
        }

        ActivateRing(d);
    }

    public void GenerateChunk(Vector3Int c)
    {
        //For each tile in the area
        //Generate new tile
        //Debug.Log("Generating more map!");

        List<Vector3Int> tiles = Hexer.GetCoordinatesInRange(c, Data.Instance.ChunkSize);
        //First is known total tiles, second is known spawns
        Dictionary<int, (int, int)> knownCounts = new Dictionary<int, (int, int)>();
        tiles.Add(c);

        foreach (Vector3Int t in tiles)
        {
            if (!tilemap.HasTile(t))
            {
                //Here's where we need to make the magic happen

                //On each ring the chance that a single tile contains a spawn is
                // p(spawn) = (total_spawns - spawns_placed) / (|ring(d)| - |known_tiles|)
                int d = Hexer.Distance(Vector3Int.zero, t);
                if (!knownCounts.ContainsKey(d))
                {
                    int known = 0;
                    int knownSpawns = 0;
                    foreach (Vector3Int v in Hexer.Ring(Vector3Int.zero, d))
                    {
                        if (tilemap.HasTile(v))
                        {
                            if (tilemap.GetTile(v).name == spawnTile.name)
                            {
                                knownSpawns++;
                            }
                            known++;
                        }
                    }
                    knownCounts[d] = (known, knownSpawns);
                    //Debug.Log($"Added key {d} to knownCounts ({known} known, {knownSpawns} known spawns)");
                }
                else
                {
                    //Debug.Log($"Already contains counts for Distance {d}");
                }

                //Debug.Log($"Params| Spawns in ring {Data.Instance.SpawnsInRing(d)} | known spawns: {knownCounts[d].Item2} | Ring size: {Hexer.RingSize(d)} | Known tiles: {knownCounts[d].Item1}");
                float spawnProb = knownCounts[d].Item1 < Hexer.RingSize(d) ? (float)(Data.Instance.SpawnsInRing(d) - knownCounts[d].Item2) / (float)(Hexer.RingSize(d) - knownCounts[d].Item1) : 0;
                //Debug.Log($"Spawn prob at d={d}: {spawnProb}");
                if (Random.Range(0f, 1f) < spawnProb)
                {
                    tilemap.SetTile(t, spawnTile);
                    knownCounts[d] = (knownCounts[d].Item1 + 1, knownCounts[d].Item2 + 1);
                }
                else
                {
                    tilemap.SetTile(t, grassTile);
                    knownCounts[d] = (knownCounts[d].Item1 + 1, knownCounts[d].Item2);
                }
            }
        }
    }
}
