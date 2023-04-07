using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperExplosion : MonoBehaviour
{
    public bool hitAlready = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hitAlready)
        {
            PlayerManger playerHealth = other.GetComponent<PlayerManger>();
            playerHealth.TakeDamage(2);
            hitAlready = true;
            Destroy(this.gameObject, 2f);
        }
    }
}
