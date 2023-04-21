using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTendral : MonoBehaviour
{
    public LineRenderer lr;
    public Transform enemy;
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, enemy.position);
    }

    public void SetEnemy(Transform transform)
    {
        enemy = transform;
    }
}
