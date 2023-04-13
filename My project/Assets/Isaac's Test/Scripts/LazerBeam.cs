using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{
    public bool useLaser = false;
    public int damageOverTime = 30;
    public GameObject target;
    public GameObject lazerStart;
    public LineRenderer lineRenderer;
    //public ParticleSystem impactEffect;
    //public Light impactLight;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
    }
    void Laser()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, lazerStart.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
        Vector3 dir = this.transform.position - target.transform.position;
    }
    void LockOnTarget()
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
        }
    }
    
