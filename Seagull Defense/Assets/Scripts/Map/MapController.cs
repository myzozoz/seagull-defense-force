using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class MapController : MonoBehaviour
{
    [SerializeField]
    private Tile pathTile;
    [SerializeField]
    private Tile grassTile;
    [SerializeField]
    private Tile fogTile;
    [SerializeField]
    private Tile spawnTile;

    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void ConstructPath(Vector3Int c)
    {
        //If tile is "path buildable" and there is an adjacent pathtile
        if (tilemap.GetTile(c).name == grassTile.name && NeighboursContain(c, pathTile))
        {
            //Set tile
            tilemap.SetTile(c, pathTile);
            //Convert nearby fog tiles to grass tiles.
            UpdateVision(c);
        }
    }

    public void SelectCell(Vector3Int c)
    {
        //Debug.Log($"Grid coordinates: {c} | Cube coordinates: {Hexer.GridToCube(c)} | Distance to origin: {Hexer.Distance(new Vector2Int(0,0), c)} | Tile: {tilemap.GetTile(new Vector3Int(c.x, c.y, 0)).name}");
    }

    private bool NeighboursContain(Vector3Int pos, Tile t)
    {
        List<Vector3Int> nList = Hexer.GetNeighbors(pos);
        foreach (Vector3Int n in nList)
        {
            if (tilemap.GetTile(n).name == t.name)
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
            if (tilemap.HasTile(a) && tilemap.GetTile(a).name == fogTile.name)
            {
                tilemap.SetTile(a, grassTile);
            }
        }
    }

    /*
    public List<Vector3> GetSpawnPositions()
    {
        BoundsInt b = tilemap.cellBounds;
        List<Vector3> spawnPos = new List<Vector3>();
        Grid g = Data.Instance.GridComponent;

        for (int x = b.xMin; x <= b.xMax; x++)
        {
            for (int y = b.yMin; y <= b.yMax; y++)
            {
                Vector3Int temp = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(temp) && tilemap.GetTile(temp).name == spawnTile.name)
                {
                    spawnPos.Add(g.CellToWorld(temp));
                }
            }
        }

        return spawnPos;
    }
    */
}
