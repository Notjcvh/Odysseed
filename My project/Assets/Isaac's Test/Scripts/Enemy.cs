using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject hitEffect;

    [Header("Animation")]
    public Animator animator;

    [Header("EnemyStatus")]
    public bool isStunned = false;
    public bool isTargeted = true;

    [Header("Rooms")]
    public CombatRoom myRoom;

    public event System.Action<float> OnHealthPercentChange = delegate {};

    private void Awake()
    {
        currentHealth = maxHealth;
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            if(myRoom != null)
            {
                myRoom.enemies.Remove(this.gameObject);
            }
            if (this.tag == "Boss")
            {
                animator.SetBool("IsDying", true);
            }
            else
            {
                this.gameObject.SetActive(false);
                DestroyImmediate(this.gameObject);
            }
            Death();
        }
    }

    // the goal here is to 

    public void WhichRoom(GameObject room) 
    {
        GameObject location = room.gameObject;
        myRoom = location.GetComponent<CombatRoom>();  // finding the Combatroom script so we can run the functions when the enemy dies
    }
    private void Death()
    {
        this.gameObject.SetActive(false);
        DestroyImmediate(this.gameObject);
    }

    public void ModifiyHealth(int amount)
    {
        GameObject hiteffs = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(hiteffs, 2f);
        currentHealth -= amount;
        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChange(currentHealthPercent);
    }

    public void TakeDamage(int damage)
    {
        animator.SetBool("TakingDamage", true);
        this.currentHealth += damage;
    }
    public void DisableAI()
    {
        this.isStunned = true;
    }



}
