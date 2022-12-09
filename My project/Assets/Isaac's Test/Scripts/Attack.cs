using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject impactEffect;
    public PlayerManger playerHealth;

    public bool hitAlready;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hitAlready)
        {
            GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);
            playerHealth = other.GetComponent<PlayerManger>();
            playerHealth.TakeDamage(1);
            hitAlready = true;
            Destroy(effectIns, 2f);
        }
    }
}
