using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotGrunt : MonoBehaviour
{
    [Header("Movement Attributes")]
    public float baseMoveSpeed;
    private float moveSpeed;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("Attack Attributes")]
    public float attackRange, sightRange;
    public bool playerInSightRange, playerinAttackRange;
    public bool isAttacking;
    public GameObject attackHitbox;

    public LayerMask whatIsGround, whatIsPlayer;
    private Animator animator;
    private NavMeshAgent nav;
    private GameObject player;

    [Header("Audio Caller")]
    private AudioController audioController;
    public AudioSource audioSource;
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
        nav = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        moveSpeed = baseMoveSpeed;

        audioController = GetComponent<AudioController>();
        SetAudio();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerinAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        nav.speed = moveSpeed;
        BasicState();
    }

    public void BasicState()
    {
        if (!playerInSightRange && !playerinAttackRange)
        {
            Walking();
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
        else if (playerInSightRange && !playerinAttackRange)
        {
            ChasePlayer();
        }
        else if (playerinAttackRange && playerInSightRange)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = true;
        nav.SetDestination(this.transform.position);
        Vector3 newTarget = player.transform.position;
        newTarget.y = transform.position.y;
        transform.LookAt(newTarget);
        animator.SetBool("isAttacking", true);
    }

    public void ChasePlayer()
    {
        nav.SetDestination(player.transform.position);
    }

    public void Walking()
    {
        moveSpeed = baseMoveSpeed;
        animator.SetBool("isWalking", true);
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            nav.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        nav.SetDestination(this.transform.position);
    }
    public void ActivateAttack()
    {
        attackHitbox.SetActive(true);
    }
    public void DeActivateAttack()
    {
        attackHitbox.SetActive(false);
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
