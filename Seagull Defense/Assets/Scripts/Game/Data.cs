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
    private int fov;

    private Grid grid;
    private MapController map;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject GridObject
    {
        get { return gridObject; }
    }

    public GameObject TilemapObject
    {
        get { return tilemapObject; }
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
}
