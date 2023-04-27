using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
using UnityEngine.Rendering;
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
    public EnemyHealthbar myHealthbar;
    public event System.Action<float> OnHealthPercentChange = delegate { };
    private BossEvents bossEvents;

    [Header("Rooms")]
    public CombatRoom myRoom;

   

    [Header("Hit Effect")]
    public float blinkIntensity;
    public float blinkDuration;
    public SkinnedMeshRenderer[] mats;
    private float blinkTimer;


    private void Awake()
    {
        currentHealth = maxHealth;
        

        if(this.tag == "Boss")
        {
          bossEvents = GetComponent<BossEvents>();
            if(bossEvents == null)
            {
                Debug.Log("you need to add boss events to change audio or spawn seed");
            }
        }
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            Vector3 enemyPos = this.transform.position;
            GameObject smokeEffect = Instantiate(GameAssets.i.smokePoof, new Vector3(enemyPos.x,enemyPos.y + 1 , enemyPos.z) , Quaternion.identity);
            if (myRoom != null)
            {
                myRoom.enemies.Remove(this.gameObject);
            }
            if (this.tag == "Boss")
            {

                animator.SetBool("isDying", true);
                Destroy(myHealthbar.gameObject);
            }
           /* else
            {
                this.gameObject.SetActive(false);
                DestroyImmediate(this.gameObject);
            }*/
            this.gameObject.SetActive(false);
            if(bossEvents != null)
            {
                     bossEvents.Call();
            }


            Destroy(smokeEffect, 1.5f);
            Destroy(this.gameObject);
        }


        /*
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;
        if(mats?.Length > 0)
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].material.color = Color.white * intensity;
            }

        }*/
       
    }

    // the goal here is to 

    public void WhichRoom(GameObject room) 
    {
        GameObject location = room.gameObject;
        myRoom = location.GetComponent<CombatRoom>();  // finding the Combatroom script so we can run the functions when the enemy dies
    }

    public void ModifiyHealth(int amount)
    {
        if (hitEffect != null)
        {
            GameObject hiteffs = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hiteffs, 2f);
        }
        currentHealth = currentHealth - amount;
        float currentHealthPercent = (float)currentHealth / (float)maxHealth;

        myHealthbar.HandleHealthChange(currentHealthPercent);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("I was hit was in my name " + this.gameObject.name);
        DamagePopUp.Create(this.transform.position, damage);
        ModifiyHealth(damage);
        DisableAI();
        blinkTimer = blinkDuration;

       // GetComponent<EnemyStats>().VisualizeDamage(this.gameObject);
    }
    public void PlayTakeDamgage()
    {
        animator.SetBool("TakingDamage", true);
    }
    public void DisableAI()
    {
        this.isStunned = true;
    }


}
