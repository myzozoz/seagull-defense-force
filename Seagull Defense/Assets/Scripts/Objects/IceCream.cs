using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IceCream : MonoBehaviour
{
    [SerializeField]
    private UnityEvent IceCreamDestroyedEvent;

    private bool free = true;

    void Start()
    {
        IceCreamDestroyedEvent.AddListener(Data.Instance.RefreshIceCreams);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (free)
        {
            Debug.Log($"Collision detected with {collider.gameObject.name}");
            Seagull sg = collider.gameObject.GetComponent<Seagull>();
            if (sg != null)
            {
                sg.SnatchBooty(this);
                free = false;
            }
        }
    }

    void OnDestroy()
    {
        IceCreamDestroyedEvent.Invoke();
    }

    public void RegisterIceCreamListener(UnityAction a)
    {
        IceCreamDestroyedEvent.AddListener(a);
    }
}
