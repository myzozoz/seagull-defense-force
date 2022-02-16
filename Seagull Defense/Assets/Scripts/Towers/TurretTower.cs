using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTower : Tower
{
    [SerializeField]
    private GameObject top;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float shotForce = 10f;
    [SerializeField]
    private float range = 5f;
    [SerializeField]
    private float secondsPerShot = 1f;

    private Transform target;
    private float shotTime = 0f;

    public override void Shoot()
    {
        if (Time.time - shotTime < secondsPerShot)
        {
            return;
        }

        GameObject po = Object.Instantiate(projectile, projectileSpawnPoint);
        Rigidbody2D rb = po.GetComponent<Rigidbody2D>();

        Vector2 shotDirection = new Vector2(projectileSpawnPoint.forward.x, projectileSpawnPoint.forward.y);
        rb.AddForce(shotDirection * shotForce, ForceMode2D.Force);

        shotTime = Time.time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {

        if (target == null || VecToTarget().magnitude > range)
        {
            UpdateTarget();
        }

        if (target == null)
        {
            return;
        }

        Vector3 lookPos = target.position;
        lookPos.z = top.transform.position.z;
        top.transform.LookAt(lookPos);

        Shoot();
    }

    private void UpdateTarget()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, range, new Vector2(), 0f, LayerMask.GetMask("Seagull"), -1f, 1f);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        } else
        {
            target = null;
        }
    }

    private Vector2 VecToTarget()
    {
        return new Vector2(target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
    }
}
