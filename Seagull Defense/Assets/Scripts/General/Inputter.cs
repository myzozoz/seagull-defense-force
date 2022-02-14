using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        map = Data.Instance.Map;
    }

    // Update is called once per frame
    void Update()
    {
        //Camera zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime, maxZoomIn, maxZoomOut);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Mouse pos: {cam.ScreenToWorldPoint(Input.mousePosition)}");
            Vector3Int gridPos = Data.Instance.GridComponent.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
            map.ConstructPath(new Vector3Int(gridPos.x, gridPos.y, 0));
        }

        if (Input.GetMouseButtonDown(1))
        {
            prevMouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = prevMouseWorldPoint - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.Translate(new Vector3(mouseDelta.x, mouseDelta.z, 0));
            prevMouseWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
