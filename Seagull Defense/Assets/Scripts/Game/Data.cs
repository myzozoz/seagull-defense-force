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
    private List<GameObject> iceCreams;
    private List<Vector3> icPos;

    // Start is called before the first frame update
    void Start()
    {
        iceCreams = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ice Cream"));
        icPos = new List<Vector3>(new Vector3[3]);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < iceCreams.Count; i++)
        {
            icPos[i] = iceCreams[i].transform.position;
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

    public List<Vector3> ICPos
    {
        get { return icPos; }
    }
}
