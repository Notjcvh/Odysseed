using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject hitEffect;

    [Header("Animation")]
    public Animator animator;
   

    [Header("Audio")]
    private bool hasPlayedAudio = false;
    public bool hasDamageAudio = false;
    public AudioSource audioSource;
    public EnemyEventCaller enemyEventCaller;

    [Header("EnemyStatus")]
    public bool isStunned = false;
    public bool isTargeted = true;
    public EnemyHealthbar myHealthbar;
    private BossEvents bossEvents;
    public bool callEvent;

    [Header("Rooms")]
    public CombatRoom myRoom;

    [Header("Hit Effect")]
    public float blinkIntensity;
    public float blinkDuration;
    public SkinnedMeshRenderer[] mats;
    private float blinkTimer;
    




    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        enemyEventCaller = GetComponent<EnemyEventCaller>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        if (this.tag == "Boss")
        {
            bossEvents = GetComponent<BossEvents>();
            if (bossEvents == null)
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
           
            if (myRoom != null)
            {
                myRoom.enemies.Remove(this.gameObject);
            }
            if (this.tag == "Boss")
            {
                animator.SetBool("isDying", true);
                if(myHealthbar != null)
                    Destroy(myHealthbar.gameObject);
            }


            if (!hasPlayedAudio)
            {
            
                GameObject smokeEffect = Instantiate(GameAssets.i.smokePoof, new Vector3(enemyPos.x, enemyPos.y + 1, enemyPos.z), Quaternion.identity);
                Destroy(smokeEffect, 1.5f);
                CallAudio("Death");
                hasPlayedAudio = true;
            }
            /* else
             {
                 this.gameObject.SetActive(false);
                 DestroyImmediate(this.gameObject);
             }*/
            //  this.gameObject.SetActive(false);
            // CallAudio();
            if (bossEvents != null && callEvent == false)
            {
                bossEvents.Call();
                callEvent = true;
            }

            if (audioSource.clip != null)
                Destroy(this.gameObject, audioSource.clip.length);
            else
                Destroy(this.gameObject);
        }
        if(isStunned)
        {
            NavMeshAgent nav = this.gameObject.GetComponent<NavMeshAgent>();
            nav.enabled = false;
            Invoke("ReEnableAi", 3);
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

    public void ReEnableAi()
    {
        NavMeshAgent nav = this.gameObject.GetComponent<NavMeshAgent>();
        nav.enabled = true;
        isStunned = false;
    }
    private void CallAudio(string keyword)
    {
        enemyEventCaller.AudioEventCalled(keyword);
    }

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
       // Debug.Log("I was hit was in my name " + this.gameObject.name);
        DamagePopUp.Create(this.transform.position, damage);
        ModifiyHealth(damage);

        if (hasDamageAudio)
            CallAudio("Damage");
        this.isStunned = true;
        DisableAI();
       // PlayTakeDamgage();
        blinkTimer = blinkDuration;
    }
    public void PlayTakeDamgage()
    {
        animator.SetBool("TakingDamage", true);
    }
    public void DisableAI()
    {
        this.isStunned = true;
    }

    public void ReturnToNormal()
    {
        Debug.Log("Returned to normal");
        rb.constraints = RigidbodyConstraints.None;
    }

    public void AppliedForce(PhysicsBehaviours appliedForce)
    {
        Debug.Log("Applied Forces Called");
        switch(appliedForce)
        {
            case PhysicsBehaviours.None:
                //slide the enemy based the direction but don't let them fall down
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
            case PhysicsBehaviours.AggresiveKnockback:
                rb.constraints = RigidbodyConstraints.None;
                break;
        }







    }














}
