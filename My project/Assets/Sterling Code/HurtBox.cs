using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    int health = 2;


    void TakeDamage(int number)
    {
        Debug.Log("Hit reciveved");

        health -= number;
    }


    private void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
