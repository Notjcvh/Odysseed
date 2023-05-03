using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopperEnemyy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAge;

    [Header("Animation")]
    public Animator animator;

    [Header("Combat")]
    public float attackRange;
    public bool isStunned = false;
    public float aggroRange;
    public float deaggroRange;
    private float distanceFromPlayer;
    public GameObject attackHitbox;
    public float stunDuration = 10;
    public bool isTargeted;
    private Enemy thisEnemy;

    [Header("Movement")]
    public float movementSpeed;
    public float idleSpeed;
    public float idleDelay;
    public Transform currentWaypoint;
    public GameObject patrolPoint;
    public Rigidbody rb;
    // Start is called before the first frame update



    [Header("Audio Caller")]
    public AudioSource audioSource;
    private AudioController audioController;
    public AudioType playingAudio; // the currently playing audio
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    public bool audioTableSet = false; // if job sent is true then it won't play
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    //Getters and Setters
    public Dictionary<AudioType, AudioClip> MyAudio { get { return ourAudio; } private set { ourAudio = value; } }
    void Awake()
    {
        currentWaypoint = this.transform;
        thisEnemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        navMeshAge = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        //currentAttackPos = attackPoints[0].GetComponent<Transform>();

        audioController = GetComponent<AudioController>();
        SetAudio();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAge.speed = movementSpeed;
        if (thisEnemy.isStunned)
        {
            DisableAI(stunDuration);
            thisEnemy.isStunned = false;
        }
        else
        {
            distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
            
            if (distanceFromPlayer <= attackRange)
            {
                //if the enemy is in attack range do this
                transform.LookAt(player.transform);
                movementSpeed = 0;
                animator.SetBool("isRunning", false);
                TriggerExplosion();
            }
            else if (distanceFromPlayer < aggroRange)
            {
                //if the enemy sees the player but is not in attack range
                navMeshAge.destination = player.transform.position;
                animator.SetBool("isRunning", true);
            }
            else
            {
                //if enemy does not see the player do this
                animator.SetBool("isRunning", true);
                navMeshAge.destination = player.transform.position;
            }
            if(distanceFromPlayer > deaggroRange)
            {
                animator.SetBool("isRunning", false);
                navMeshAge.destination = this.transform.position;
            }
        }
    }

    public void TriggerExplosion()
    {
        animator.SetBool("isExploding", true);
    }
    public void Explode()
    {
        if(distanceFromPlayer < attackRange)
        {
            Instantiate(attackHitbox, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            animator.SetBool("isExploding", false);
            navMeshAge.destination = player.transform.position;
            movementSpeed = 10;
        }
    }
    public void DisableAI(float duration)
    {
        this.navMeshAge.enabled = false;
        isStunned = true;
        Invoke("EnableAI", Time.deltaTime * duration);
    }

    public void EnableAI()
    {
        isStunned = false;
        this.navMeshAge.enabled = true;
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
