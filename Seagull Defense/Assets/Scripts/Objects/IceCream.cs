using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log($"Collision detected with {collider.gameObject.name}");
        Seagull sg = collider.gameObject.GetComponent<Seagull>();
        if (sg != null)
        {
            sg.SnatchBooty(transform);
        }
    }
}
