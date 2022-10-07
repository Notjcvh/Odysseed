using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlayerStats")]
public class PlayerStats: ScriptableObject
{
    

    //health
    public int maxHealth = 3;
    int currentHealth;

    //weight 
    public float playerweight = 10f;
   
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {        
        if(currentHealth <= 0)
        {
            // send a message to destory player GameObject
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

    }
}
