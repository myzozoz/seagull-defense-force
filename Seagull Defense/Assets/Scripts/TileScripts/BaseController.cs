using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> iceCreams;
    [SerializeField]
    private float ICHoverDistance = .1f;
    // Start is called before the first frame update
    void Start()
    {
        //Spawn the ice creams (delicious)
        for(int i = 0; i < iceCreams.Count; i++)
        {
            float rad = i * (2 * Mathf.PI / iceCreams.Count) - Mathf.PI/2;
            Vector3 spawnPos = new Vector3(Mathf.Cos(rad) * ICHoverDistance, Mathf.Sin(rad) * ICHoverDistance, -1f);
            GameObject.Instantiate(iceCreams[i], spawnPos, Quaternion.identity);
            //new Quaternion(0.707106829f, 0, 0, 0.707106829f)
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
