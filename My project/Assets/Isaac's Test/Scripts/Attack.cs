using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject impactEffect;
    public PlayerStats playerHealth;
    public bool hitAlready;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hitAlready)
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            playerHealth.health -= 1;
            hitAlready = true;
            Destroy(effectIns, 2f);
        }
    }
}
