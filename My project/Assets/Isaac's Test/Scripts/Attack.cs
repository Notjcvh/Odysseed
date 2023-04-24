using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject impactEffect;
    public PlayerManger playerManager;
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

        Debug.Log(other.name);
        if (other.gameObject.tag == "Shield" && !hitAlready && !hitShield)
        {
            //instatiate shield hit effect
            //GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);

            Debug.Log("attacked");
            Rigidbody body = this.GetComponent<Rigidbody>();
            playerManager.Blocked(body);
            hitAlready = true;
            hitShield = true;
            //Destroy(effectIns, 2f);
        }
        if (other.gameObject.tag == "Player" && !hitAlready && !hitShield)
        {
            GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);
            playerManager = other.GetComponent<PlayerManger>();
            playerManager.TakeDamage(damage);
            hitAlready = true;
            Destroy(effectIns, 2f);
        }

    }
}
