using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullSpawnController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Seagull sg = collider.gameObject.GetComponent<Seagull>();
        if (sg != null)
        {
            Debug.Log("Seagull just entered the spawnosphere");
            sg.OnSpawnEnter();
        }
    }
}
