using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IceCream : MonoBehaviour
{
    [SerializeField]
    private UnityEvent IceCreamDestroyedEvent;

    private bool free = true;
    private bool forceCheckNextUpdate = false;

    void Start()
    {
        IceCreamDestroyedEvent.AddListener(Data.Instance.RefreshIceCreams);
    }

    void FixedUpdate()
    {
        if (forceCheckNextUpdate)
        {
            CheckForGulls();
            forceCheckNextUpdate = false;
        }
    }

    private void CheckForGulls()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, .5f, new Vector2(), 0f, LayerMask.GetMask("Seagull"), -1f, 1f);
        if (hits.Length > 0)
        {
            Seagull sg = hits[0].transform.GetComponent<Seagull>();
            if (sg != null)
            {
                sg.SnatchBooty(this);
                free = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (free)
        {
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

    public void Drop()
    {
        transform.SetParent(null);
        free = true;
        forceCheckNextUpdate = true;
    }
}
