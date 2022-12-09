using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
    public GameObject enemy;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Vector3 spawnLocation = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            GameObject newEnemy = Instantiate(enemy, spawnLocation, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
