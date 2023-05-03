using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
    public GameObject enemy;
    public PotatoKing pk;

    private void Start()
    {
        pk = GameObject.FindGameObjectWithTag("Boss").GetComponent<PotatoKing>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Vector3 spawnLocation = new Vector3(this.transform.position.x, collision.transform.position.y, this.transform.position.z);
            GameObject newEnemy = Instantiate(enemy, spawnLocation, transform.rotation);
            pk.enemies.Add(newEnemy);
            Destroy(this.gameObject);
        }
    }
}
