using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject impactEffect;
    public PlayerManger playerHealth;
    public int damage = 1;
    public bool hitAlready;
    public bool hitShield;
    private void OnEnable()
    {
        hitAlready = false;
        hitShield = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Shield" && !hitAlready && !hitShield)
        {
            //instatiate shield hit effect
            //GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);

            //find shield script and damge it
            //playerHealth = other.GetComponent<PlayerManger>();
            //playerHealth.TakeDamage(1);
            hitAlready = true;
            hitShield = true;
            //Destroy(effectIns, 2f);
        }
        if (other.gameObject.tag == "Player" && !hitAlready && !hitShield)
        {
            GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);
            playerHealth = other.GetComponent<PlayerManger>();
            playerHealth.TakeDamage(damage);
            hitAlready = true;
            Destroy(effectIns, 2f);
        }

    }
}
