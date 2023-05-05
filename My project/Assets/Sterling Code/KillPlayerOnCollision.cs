using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerManger>().HitKillFloor();
        }



    }
}
