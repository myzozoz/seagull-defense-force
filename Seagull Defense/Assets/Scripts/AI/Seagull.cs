using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Seagull : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    private bool hasIce = false;
    private Vector3 target;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (target == new Vector3(0,0,0))
        {
            Debug.Log("No target");
            UpdateTarget();
        }

        if (hasIce && Mathf.Abs(transform.position.x - target.x) < .01 && Mathf.Abs(transform.position.y - target.y) < .01)
        {
            Debug.Log("Ice stolen get mad");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log($"Distance remaining to target: {target - transform.position}");
        }

        Vector3 move = target - transform.position;
        move.z = 0;
        rb.MovePosition(transform.position + (move.normalized * moveSpeed * Time.deltaTime));
        //rb.MoveRotation(rb.rotation + Vector3.Angle(transform.right, move));

        
        Vector3 lookDir = -transform.up;
        float rotAngle = Vector3.Angle(lookDir, move);
        float rotDir = Vector3.Dot(transform.right, move); // pos is on the left side
        rotAngle *= rotDir > 0 ? 1 : -1;
        //Debug.Log($"fwd: {lookDir} | target: {target} | angle: {rotAngle} | rot dir: {rotDir}");
        //positive value turns counter-clockwise
        rb.MoveRotation(rb.rotation + rotAngle);
    }

    protected void UpdateTarget()
    {
        if (!hasIce)
        {
            target = FindClosest(Data.Instance.ICPos);
        }
        else
        {
            target = FindClosest(Data.Instance.Map.GetSpawnPositions());
        }
        Debug.Log($"Target updated, now headed to {target}");
        
    }

    public void SnatchBooty(Transform tf)
    {
        Debug.Log("Rooty tooty we got the booty!");
        hasIce = true;
        tf.SetParent(transform);
        tf.localPosition = new Vector3(0f, -0.3f, 0f);
        tf.localEulerAngles = new Vector3(0f, 0f, 115f);
        UpdateTarget();
    }

    private Vector3 FindClosest(List<Vector3> targets)
    {
        float min_dist = 1000000f;
        Vector3 min_t = new Vector3();
        foreach (Vector3 t in targets)
        {
            float dist = Vector3.Distance(transform.position, t);
            if (dist < min_dist)
            {
                min_dist = dist;
                min_t = t;
            }
        }
        return min_t;
    }
}
