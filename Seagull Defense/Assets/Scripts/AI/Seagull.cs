using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public abstract class Seagull : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float turnAmplitude;
    [SerializeField]
    [Tooltip("Maximum multiplier, 1 means that values are between 0 and 2 * Turn Amplitude")]
    private float turnAmplitudeSpread;
    [SerializeField]
    private float turnFrequency;
    [SerializeField]
    [Tooltip("Maximum multiplier, 1 means that values are between 0 and 2 * Turn Frequency")]
    private float turnFrequencySpread;

    protected bool hasIce = false;
    private Transform target;
    private Rigidbody2D rb;
    private IceCream iceCream = null;
    private float health;
    private bool updateTargetFlag = false;
    private float turnOffset;

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

        turnAmplitude += Random.Range(-turnAmplitudeSpread, turnAmplitudeSpread) * turnAmplitude;
        turnFrequency += Random.Range(-turnFrequencySpread, turnFrequencySpread) * turnFrequency;
        turnOffset = Random.Range(0, 2 * Mathf.PI / turnFrequency);
        //Debug.Log($"Offset values| Amplitude: {turnAmplitude} | Frequency {turnFrequency} | turnOffset");
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

        Move();
    }

    private void Move()
    {
        Vector3 move = target.position - transform.position;
        move.z = 0;
        move = move.normalized;

        float angle = turnAmplitude * Mathf.Sin(turnOffset + Time.time * Mathf.PI * turnFrequency);

        move = new Vector3(Mathf.Cos(angle) * move.x - Mathf.Sin(angle) * move.y,
            Mathf.Sin(angle) * move.x + Mathf.Cos(angle) * move.y,
            0);

        rb.MovePosition(transform.position + (move * moveSpeed * Time.deltaTime));

        //The part about turning
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
            List<GameObject> spawns = Data.Instance.GullSpawns.ToList();
            Debug.Log($"Got the ice, {spawns.Count} spawns found");
            target = FindClosest(spawns);
        }
        //Debug.Log($"Target updated, now headed to {target}");
    }

    private Transform FindClosest(List<GameObject> targets)
    {
        float min_dist = 1000000f;
        Transform min_t = transform;
        foreach (GameObject go in targets)
        {
            if (go == null)
            {
                continue;
            }
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
            //Debug.Log("Ice stolen get mad");
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float val)
    {
        health = Mathf.Clamp(health - val, 0f, maxHealth);
        //Debug.Log($"Took {val} damage ({health}/{maxHealth} remaining)");
        if (health <= 0f)
        {
            Data.Instance.Bank.ChangeBalance(5);
            Die();
        }
    }

    public void Die()
    {
        //Debug.Log($"Child count at the time of death: {transform.childCount}");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.tag == "Ice Cream")
            {
                child.GetComponent<IceCream>().Drop();
            }
        }
        Destroy(this.gameObject);
    }

    public bool HasIce
    {
        get { return hasIce; }
    }
}
