using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAi : MonoBehaviour
{
    [Header("Generic Attributes")]
    public float genericAtkSpeed;
    private float genericAtkSpeedCounter;
    public GameObject hitEffect;
    public Transform hiteffectLocation;
    [Header("Movement")]
    public float movementSpeed;
    private float tempAttackMoveSpeed;
    public float angularSpeed;
    public Rigidbody rb;
    private float tempAngularSpeed;
    [Header("Ranged Attributes")]
    public float attack1Speed;
    public GameObject attack1;
    public float attack1Life;
    [Header("Melee Attributes")]
    public float meleeCooldown;
    public float attack2Life;
    public float meleeRange;
    private float meleeCooldownCounter;
    public GameObject attackHitbox;
    public GameObject explosion;
    public Attack attackScript;
    private bool activateMelee;
    private float attack2LifeCounter;
    [Header("Summon Enemy Attributes")]
    public float summonEnemyCooldown;
    private float summonEnemyCounter;
    public Transform projectileSpawnLocation;
    public GameObject projectile;
    public float projectileVelocity;
    public GameObject enemy;
    private bool isTossing;
    private GameObject player;
    public float distanceFromPlayer;
    private Animator animator;
    private NavMeshAgent navMeshAge;
    public GameObject walkingAttackHitbox;
    public GameObject walkingAttackHitbox1;

    [Header("Audio Caller")]
    public AudioSource audioSource;
    private AudioController audioController;
    public AudioType playingAudio; // the currently playing audio
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    public bool audioTableSet = false; // if job sent is true then it won't play
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    //Getters and Setters
    public Dictionary<AudioType, AudioClip> MyAudio { get { return ourAudio; } private set { ourAudio = value; } }




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAge = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        tempAngularSpeed = angularSpeed;
        rb = this.GetComponent<Rigidbody>();
        tempAttackMoveSpeed = movementSpeed;
        audioSource = GetComponent<AudioSource>();
        audioController = GetComponent<AudioController>();
        SetAudio();
    }


    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        genericAtkSpeedCounter -= Time.deltaTime;
        meleeCooldownCounter -= Time.deltaTime;
        attack2LifeCounter -= Time.deltaTime;
        summonEnemyCounter -= Time.deltaTime;
        navMeshAge.destination = player.transform.position;
        navMeshAge.speed = tempAttackMoveSpeed;
        navMeshAge.angularSpeed = tempAngularSpeed;
        if (genericAtkSpeedCounter <= 0)
        {
            Attack1();
            genericAtkSpeedCounter = genericAtkSpeed;
        }
        if(distanceFromPlayer <= meleeRange && meleeCooldownCounter <= 0)
        {
            activateMelee = true;
        }
        if (activateMelee)
        {
            animator.SetBool("IsSlaming", true);
            tempAttackMoveSpeed = 0;
            tempAngularSpeed = 0;
            attack2LifeCounter = attack2Life;
            meleeCooldownCounter = meleeCooldown;
            activateMelee = false;
        }
        if (summonEnemyCounter <= 0 && !isTossing)
        {
            animator.SetBool("IsTossing", true);
            tempAttackMoveSpeed = 0;
            tempAngularSpeed = 0;
            isTossing = true;
        }
        if (distanceFromPlayer <= meleeRange)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    public void Attack1()
    {
        Invoke("SpawnAttack1", attack1Speed);
        Invoke("SpawnAttack1", attack1Speed*2);
        Invoke("SpawnAttack1", attack1Speed*3);
    }

    public void SpawnAttack1()
    {
        Vector3 spawnLocation = new Vector3(player.transform.position.x, 20, player.transform.position.z);
        GameObject inGameAttack1 = Instantiate(attack1, spawnLocation, player.transform.rotation);
        Destroy(inGameAttack1, attack1Life);
    }

    public void FireEnemy()
    {
        GameObject Seed = Instantiate(projectile, projectileSpawnLocation.position, transform.rotation);
        Rigidbody rb = Seed.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileVelocity,ForceMode.Impulse);
        summonEnemyCounter = summonEnemyCooldown;
    }

    public void SummonEnemy()
    {
        GameObject inGameAttack1 = Instantiate(projectile, projectileSpawnLocation.transform.position, projectileSpawnLocation.transform.rotation);
    }

    public void ActivateMelee()
    {
        attackHitbox.SetActive(true);
        GameObject effectIns = (GameObject)Instantiate(explosion, attackHitbox.transform.position, attackHitbox.transform.rotation);
        Destroy(effectIns, 2f);
    }
    public void ReEnableMovement()
    {        
        tempAttackMoveSpeed = movementSpeed;
        tempAngularSpeed = angularSpeed;
    }
    public void EndOfMelee()
    {
        attackHitbox.SetActive(false);
        animator.SetBool("IsSlaming", false);
        attackScript.hitAlready = false;
    }
    public void EndOfToss()
    {
        animator.SetBool("IsTossing", false);
        isTossing = false;
    }

    public void WalkingAttack()
    {
        walkingAttackHitbox.SetActive(true);
        GameObject effectIns = (GameObject)Instantiate(explosion, walkingAttackHitbox.transform.position, walkingAttackHitbox.transform.rotation);
        Destroy(effectIns, 2f);
    }
    public void DeWalkingAttack()
    {
        walkingAttackHitbox.SetActive(false);
    }

    public void WalkingAttack1()
    {
        walkingAttackHitbox1.SetActive(true);
        GameObject effectIns = (GameObject)Instantiate(explosion, walkingAttackHitbox1.transform.position, walkingAttackHitbox1.transform.rotation);
        Destroy(effectIns, 2f);
    }
    public void DeWalkingAttack1()
    {
        walkingAttackHitbox1.SetActive(false);
    }

    #region Sound Caller
    private void SetAudio()
    {
        // Loop through each audio track
        foreach (AudioController.AudioTrack track in audioController.tracks)
        {
            // Access the audio objects in each track
            audioObjects.AddRange(track.audio);
            // Loop through each audio object in the track
            foreach (AudioController.AudioObject audioObject in audioObjects)
            {
                // this should add all our audio to the dictionary
                ourAudio.Add(audioObject.type, audioObject.clip);
            }
        }
        audioTableSet = true;
    }

    public void ManageAudio(AudioType type)
    {
        if (ourAudio.Count > 0 && ourAudio.ContainsKey(type) && audioController != null)
        {
            if (type != playingAudio && audioSource.isPlaying)
            {
                audioController.StopAudio(playingAudio, false, 0, false);
                playingAudio = type;
                audioController.PlayAudio(playingAudio, false, 0, false);
            }
            else
            {
                playingAudio = type;
                audioController.PlayAudio(playingAudio, false, 0, false);
            }
        }
    }
    #endregion
}
