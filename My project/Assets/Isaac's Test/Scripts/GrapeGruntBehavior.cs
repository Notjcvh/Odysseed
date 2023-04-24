using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrapeGruntBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerShield;
    private NavMeshAgent navMeshAge;
    private AudioController audioController;

    [Header("Animation")]
    public Animator animator;

   
    [Header("Combat")]
    public float attackRange;
    public float shieldAttackRange;
    public float attackSpeed;
    public float attackLife;
    public float attackPosChangeTimer;
    public bool isStunned = false;
    public Attack attackScript;
    private Transform currentAttackPos;
    private GameObject[] attackPoints;
    private int attackPointer = 0;
    public float aggroRange;
    public float deaggroRange;
    private float distanceFromPlayer;
    private float distanceFromShield;
    public GameObject attackHitbox;
    public float stunDuration = 10;
    public bool isTargeted;
    private float attackLifetime;
    private float attackCooldown;
    private Enemy thisEnemy;

    [Header("Movement")]
    public float movementSpeed;
    public float attackMoveSpeed;
    private float tempAttackMoveSpeed;
    public float idleSpeed;
    public float idleDelay;
    public Transform currentWaypoint;
    public GameObject patrolPoint;
    public Rigidbody rb;
    // Start is called before the first frame update

    [Header("Audio Caller")]
    public AudioType playingAudio; // the currently playing audio
    [SerializeField] private AudioType queueAudio; // the next audio to play
    public AudioClip clip;
    public bool audioJobSent = false; // if job sent is true then it won't play
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();


    void Awake()
    {
        currentWaypoint = this.transform;
        thisEnemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        navMeshAge = GetComponent<NavMeshAgent>();
        attackCooldown = attackSpeed;
        tempAttackMoveSpeed = attackMoveSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
      
        attackPoints = GameObject.FindGameObjectsWithTag("AttackPoints");
        rb = this.GetComponent<Rigidbody>();
        currentAttackPos = player.transform;
        //currentAttackPos = attackPoints[0].GetComponent<Transform>();

        audioController = GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioController != null)
        {
           //audioController.PlayAudio(AudioType.RotEnemyNoise, false, 0, false);
        }

        if (thisEnemy.isStunned)
        {
            //DisableAI(stunDuration);
            thisEnemy.isStunned = false;
        }
        else
        {
            distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);

            //For the shield
            playerShield = GameObject.FindGameObjectWithTag("Shield");
            if (playerShield != null && playerShield.activeInHierarchy == true)
            {
                distanceFromShield = Vector3.Distance(this.transform.position, playerShield.transform.position);
               // Debug.Log(distanceFromPlayer);
            }

            attackCooldown -= Time.deltaTime;
            attackLifetime -= Time.deltaTime;
            animator.SetFloat("Speed", rb.velocity.magnitude);
            if (attackPointer == 5)
            {
                attackPointer = 0;
            }
            if (attackLifetime <= 0)
            {
                attackHitbox.SetActive(false);
                animator.SetBool("IsAttacking", false);
                attackScript.hitAlready = false;
                tempAttackMoveSpeed = attackMoveSpeed;
            }

            if(distanceFromShield <= shieldAttackRange && playerShield != null)
            {
                //if the enemy is in attack range do this
                navMeshAge.speed = tempAttackMoveSpeed;
                transform.LookAt(player.transform);
                navMeshAge.destination = playerShield.transform.position;
                if (attackCooldown <= 0)
                {
                    attackHitbox.SetActive(true);
                    animator.SetBool("IsAttacking", true);
                    tempAttackMoveSpeed = 0;
                    attackLifetime = attackLife;
                    attackCooldown = attackSpeed;
                }
            }

            if (distanceFromPlayer <= attackRange)
            {
                //if the enemy is in attack range do this
                navMeshAge.speed = tempAttackMoveSpeed;
                transform.LookAt(player.transform);
                navMeshAge.destination = currentAttackPos.position;
                if (attackCooldown <= 0)
                {
                    attackHitbox.SetActive(true);
                    animator.SetBool("IsAttacking", true);
                    tempAttackMoveSpeed = 0;
                    attackLifetime = attackLife;
                    attackCooldown = attackSpeed;
                }
            }
            else if (distanceFromPlayer < aggroRange)
            {
                //if the enemy sees the player but is not in attack range

                //Debug.Log("Deaggro");
                aggroRange = deaggroRange;
                navMeshAge.speed = tempAttackMoveSpeed;
                navMeshAge.destination = currentAttackPos.position;
            }
            else
            {
                //if enemy does not see the player do this
               // Debug.Log("Can't see the player ");
                navMeshAge.destination = currentWaypoint.position;
                navMeshAge.speed = idleSpeed;
            }
        }
    }

    //public void DisableAI(float duration)
    //{
    //    this.navMeshAge.enabled = false;
    //    attackCooldown = 99999f;
    //    isStunned = true;
    //    Invoke("EnableAI", Time.deltaTime * duration);
    //}

    //public void EnableAI()
    //{
    //    attackCooldown = attackSpeed;
    //    isStunned = false;
    //    this.navMeshAge.enabled = true;
    //}



    #region Sound Caller

    public void ManageAudio(AudioType type)
    {
        if (ourAudio.Count < 1)
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
        }
        if (ourAudio.ContainsKey(type))
        {
            clip = ourAudio[type];
        }
        if (type != playingAudio)
        {
            audioController.PlayAudio(type, false, 0, false);
            playingAudio = type;
        }
        else
        {
            audioController.PlayAudio(playingAudio, false, 0, false);
        }
    }

    IEnumerator WaitToPlay(float time, AudioType type)
    {
        float count = 0;
        count += 1;
        yield return new WaitForSecondsRealtime(time);
        ManageAudio(type);
        Debug.Log("count " + count);
    }

    #endregion
}
