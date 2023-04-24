using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeam : Abilites
{
    public bool useLaser = false;
    public float sunBeamDuration;
    public int damageOverTime = 30;
    public Transform target;
    public GameObject lazerStart;
    public LineRenderer lineRenderer;
    public float range;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(useLaser && target != null)
        {
            lineRenderer.SetPosition(0, lazerStart.transform.position);
            lineRenderer.SetPosition(1, target.position);
        }
        UpdateTarget();
    }

    public override void Ability()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer; 
        useLaser = true;
        StartCoroutine("EndSunBeam", sunBeamDuration);
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
            nearestEnemy.GetComponent<Enemy>().currentHealth -= damageOverTime;
        }
        if (useLaser)
        {
            player.transform.LookAt(nearestEnemy.transform);
        }
        else
        {
            target = null;
        }
    }
    private IEnumerator EndSunBeam(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(lineRenderer);
        useLaser = false;
    }
}
