using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrotWarden : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    private NavMeshAgent age;
    private bool isMoving;
    private GameObject player;
    [Header("Attack attributes")]
    public int attackCounter = 0;
    public List<GameObject> attackHitboxes;
    private bool hasAttacked;
    private bool playerNearbyEndsAnimation;
    private bool playerInAttackRange;
    public float attackRange;
    private bool isAttacking = false;
    public LayerMask whatIsPlayer;
    public GameObject rangedProjectile;
    public Transform projectileSpawnLocation;
    public float projectileVelocity;
    public float projectileLifetime;
    private Animator animator;


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
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        age = this.GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioController = GetComponent<AudioController>();
        SetAudio();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!isAttacking)
        {
            switch (attackCounter)
            {
                case (0):
                    Idles();
                    break;
                case (1):
                    Walking();
                    break;
                case (2):
                    Attacking();
                    break;
                case (3):
                    Ranged();
                    break;
                default:
                    break;
            }
        }
        if (!isMoving)
        {
            age.SetDestination(player.transform.position);
            age.speed = 0f;
        }
        else
        {
            age.SetDestination(player.transform.position);
        }
        if (playerNearbyEndsAnimation)
        {
            if (playerInAttackRange)
            {
                Idle();
                playerNearbyEndsAnimation = false;
            }
        }
        Vector3 newTarget = player.transform.position;
        newTarget.y = 0;
        transform.LookAt(newTarget);
    }

    #region Walk
    public void Walking()
    {
        animator.SetBool("isWalking", true);
        age.speed = movementSpeed;
        playerNearbyEndsAnimation = true;
        isMoving = true;
        isAttacking = true;
    }
    #endregion

    #region Slashing
    public void Attacking()
    {
        Debug.Log("Im slashing");
        if(hasAttacked)
        {
            animator.SetBool("isAttacking", true);
            hasAttacked = false;
        }   
        else
        {
            animator.SetBool("isSlamming", true);
            hasAttacked = true;
        }
        isAttacking = true;
    }
    public void EnableAttackHitbox()
    {
        attackHitboxes[0].SetActive(true);
    }
    public void DIsableAttackHitbox()
    {
        attackHitboxes[0].SetActive(false);
    }
    #endregion

    #region Ranged
    public void Ranged()
    {
        animator.SetBool("isRanged", true);
        isAttacking = true;
    }
    public void SpawnRangedProjectile()
    {
        GameObject projectile = Instantiate(rangedProjectile, projectileSpawnLocation.position, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileVelocity, ForceMode.Impulse);
        Destroy(projectile, projectileLifetime);
    }
    #endregion

    public void Death()
    {
        if (animator.GetBool("isDying") == true)
            Destroy(this.gameObject);
    }
    public void Idles()
    {
        animator.SetBool("isIdling", true);
        isAttacking = true;
    }
    public void EndChain()
    {
        attackCounter = 1;
        isAttacking = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRanged", false);
    }
    public void endIdle()
    {
        animator.SetBool("isIdling", false);
        isAttacking = false;
    }
    public void Idle()
    {
        attackCounter += 1;
        isAttacking = false;
        isMoving = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRanged", false);
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
