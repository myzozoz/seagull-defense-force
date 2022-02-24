using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inputter : GenericSingleton<Inputter>
{
    [SerializeField]
    private float zoomSpeed = 1f;
    [SerializeField]
    private float maxZoomIn = 1f;
    [SerializeField]
    private float maxZoomOut = 100f;
    
    private Camera cam;
    private MapController map;
    private Vector3 prevMouseWorldPoint;
    private Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        map = Data.Instance.Map;
        manager = Manager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        //Camera zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime, maxZoomIn, maxZoomOut);
        }

        //If mouse is on UI element, we don't react to main clicks
        if (Input.GetMouseButtonDown(0))
        {
            if (!TestMouseOverUI())
            {
                manager.Interact(Interaction.Primary, PointerToGridCoordinate());
            }
        }
            

        /*
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log($"Mouse pos: {cam.ScreenToWorldPoint(Input.mousePosition)}");
            
        }
        */

        //Map scrolling
        if (Input.GetMouseButtonDown(2))
        {
            prevMouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 mouseDelta = prevMouseWorldPoint - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.Translate(new Vector3(mouseDelta.x, mouseDelta.y, 0f));
            prevMouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private bool TestMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static Vector3Int PointerToGridCoordinate()
    {
        Vector3Int gridPos = Data.Instance.GridComponent.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return new Vector3Int(gridPos.x, gridPos.y, 0);
    }
}
