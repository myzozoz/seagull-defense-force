using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        transform.rotation = Quaternion.identity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Seagull sg = col.transform.GetComponent<Seagull>();
        if (sg != null)
        {
            sg.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
