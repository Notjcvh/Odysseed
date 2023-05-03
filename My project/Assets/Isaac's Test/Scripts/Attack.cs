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

        
       /* if (other.gameObject.tag == "Shield" && !hitAlready && !hitShield)
        {
            //instatiate shield hit effect
            //GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);
            playerManager = other.GetComponent<PlayerManger>();
            Debug.Log("attacked");
            Rigidbody body = this.GetComponent<Rigidbody>();
            playerManager.Blocked(body, damage);
            hitAlready = true;
            hitShield = true;
            //Destroy(effectIns, 2f);
        }*/
        if (other.gameObject.tag == "Player" && !hitAlready)
        {
            GameObject effectIns = Instantiate(GameAssets.i.hit1, other.transform.position, Quaternion.identity);
            playerManager = other.GetComponent<PlayerManger>();
            playerManager.TakeDamage(damage);
            hitAlready = true;
            Destroy(effectIns, 2f);
        }

    }
}
