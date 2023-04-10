using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeam : Abilites
{
    public bool useLaser = false;
    public int damageOverTime = 30;
    public Transform target;
    public GameObject lazerStart;
    public LineRenderer lineRenderer;
    public float range;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if(useLaser)
        {
            lineRenderer.SetPosition(0, lazerStart.transform.position);
            lineRenderer.SetPosition(1, target.position);
        }
    }

    public override void Ability()
    {
        useLaser = true;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}
