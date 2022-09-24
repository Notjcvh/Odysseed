using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] GameObject player;

    //variables
    int maxHealth = 3;
    public int currentHealth;
    int checkvalue;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        checkvalue = currentHealth;
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(1);
            Debug.Log(currentHealth);
        }
        
        /*if(currentHealth <= 0)
        {
            Destroy(player);
        }*/
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
