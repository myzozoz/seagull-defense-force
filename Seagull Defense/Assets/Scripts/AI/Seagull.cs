using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Seagull : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float maxHealth;

    protected bool hasIce = false;
    private Transform target;
    private Rigidbody2D rb;
    private IceCream iceCream = null;
    private float health;
    private bool updateTargetFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        List<GameObject> ics = GameObject.FindGameObjectsWithTag("Ice Cream").ToList();
        foreach (GameObject go in ics)
        {
            IceCream ic = go.GetComponent<IceCream>();
            ic.RegisterIceCreamListener(RequireTargetUpdate);
        }
    }

    void FixedUpdate()
    {
        if (target == null || target.position == new Vector3(0, 0, 0))
        {
            UpdateTarget();
        }
        else
        {
            //Debug.Log($"Distance remaining to target: {target.position - transform.position}");
        }

        Vector3 move = target.position - transform.position;
        move.z = 0;
        rb.MovePosition(transform.position + (move.normalized * moveSpeed * Time.deltaTime));


        Vector3 lookDir = -transform.up;
        float rotAngle = Vector3.Angle(lookDir, move);
        float rotDir = Vector3.Dot(transform.right, move); // pos is on the left side
        rotAngle *= rotDir > 0 ? 1 : -1;
        //Debug.Log($"fwd: {lookDir} | target: {target} | angle: {rotAngle} | rot dir: {rotDir}");
        //positive value turns counter-clockwise
        rb.MoveRotation(rb.rotation + rotAngle);
    }

    void LateUpdate()
    {
        if (updateTargetFlag)
        {
            UpdateTarget();
            updateTargetFlag = false;
        }
    }

    private void RequireTargetUpdate()
    {
        updateTargetFlag = true;
    }

    protected void UpdateTarget()
    {
        if (!hasIce)
        {
            target = FindClosest(Data.Instance.IceCreams.ToList());
        }
        else
        {
            target = FindClosest(Data.Instance.GullSpawns.ToList());
        }
        //Debug.Log($"Target updated, now headed to {target}");
    }

    private Transform FindClosest(List<GameObject> targets)
    {
        float min_dist = 1000000f;
        Transform min_t = transform;
        foreach (GameObject go in targets)
        {
            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < min_dist)
            {
                min_dist = dist;
                min_t = go.transform;
            }
        }
        return min_t;
    }

    public void SnatchBooty(IceCream ic)
    {
        if (!hasIce)
        {
            Debug.Log("Rooty tooty we got the booty!");
            hasIce = true;
            ic.transform.SetParent(transform);
            ic.transform.localPosition = new Vector3(0f, -0.3f, 0f);
            ic.transform.localEulerAngles = new Vector3(0f, 0f, 115f);
            UpdateTarget();
            iceCream = ic;
        }
    }

    public void OnSpawnEnter()
    {
        if (hasIce)
        {
            Debug.Log("Ice stolen get mad");
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float val)
    {
        health = Mathf.Clamp(health - val, 0f, maxHealth);
        Debug.Log($"Took {val} damage ({health}/{maxHealth} remaining)");
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
