using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class carrotKhan : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float spinSpeed;
    public float chargeSpeed;
    private NavMeshAgent age;
    private bool isMoving;
    private GameObject player;
    [Header("Attack attributes")]
    public int attackCounter = 0;
    public List<GameObject> attackHitboxes;
    private bool playerNearbyEndsAnimation;
    private bool playerInAttackRange;
    public float attackRange;
    private bool isAttacking = false;
    public int healthThreshold = 50;
    private Enemy enemyScript;
    public LayerMask whatIsPlayer;
    [Header("LazerBeam")]
    private LazerBeam lb;
    [Header("Phase 2")]
    public bool hasExploded = false;
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
        enemyScript = this.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        age = this.GetComponent<NavMeshAgent>();
        lb = this.GetComponent<LazerBeam>();
        audioSource = GetComponent<AudioSource>();
        audioController = GetComponent<AudioController>();
        SetAudio();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!isAttacking && !hasExploded)
        {
            switch(attackCounter)
            {
                case (0):
                    Idles();
                    break;
                case (1):
                    Taunt();
                    break;
                case (2):
                    Run();
                    break;
                case (3):
                    Slashing();
                    break;
                case (4):
                    Whirlwind();
                    break;
                case (5):
                    OverHeadSwing();
                    break;
                default:
                    break;
            }
        }
        if(!hasExploded && enemyScript.currentHealth < healthThreshold)
        {
            Explode();
        }
        if (!isAttacking && hasExploded)
        {
            switch (attackCounter)
            {
                case(0):
                    Idle2();
                    break;
                case (1):
                    Charge();
                    break;
                case (2):
                    SlamAttack();
                    break;
                case (3):
                    Run2();
                    break;
                case (4):
                    Punching();
                    break;
                case (5):
                    LazerBeam();
                    break;
                default:
                    break;
            }
        }
        if(!isMoving)
        {
            age.SetDestination(player.transform.position);
            age.speed = 0f;
        }
        else
        {
            age.SetDestination(player.transform.position);
        }
        if(playerNearbyEndsAnimation)
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
    //Phase 1
    #region Taunt
    public void Taunt()
    {
        Debug.Log("Im taunting");
        animator.SetBool("isTaunting", true);
        isAttacking = true;
    }
    #endregion

    #region Run
    public void Run()
    {
        Debug.Log("Im running");
        animator.SetBool("isRunning", true);
        age.speed = movementSpeed;
        playerNearbyEndsAnimation = true;
        isMoving = true;
        isAttacking = true;
    }
    #endregion

    #region Slashing
    public void Slashing()
    {
        Debug.Log("Im slashing");
        animator.SetBool("isSlashing", true);
        isAttacking = true;
    }
    public void EnableSlashHitbox()
    {
        attackHitboxes[0].SetActive(true);
    }
    public void DIsableSlashHitbox()
    {
        attackHitboxes[0].SetActive(false);
    }
    #endregion

    #region Whirlwind
    public void Whirlwind()
    {
        Debug.Log("Im whirlwinding");
        animator.SetBool("isWhirlwinding", true);
        age.speed = spinSpeed;
        isMoving = true;
        isAttacking = true;
    }
    public void EnableWhirlwindHitbox()
    {
        attackHitboxes[1].SetActive(true);
    }
    public void DssableWhirlwindHitbox()
    {
        attackHitboxes[1].SetActive(false);
    }
    #endregion

    #region OverHeadSwing
    public void OverHeadSwing()
    {
        Debug.Log("Im swinging");
        animator.SetBool("isOvrSwinging", true);
        isAttacking = true;
    }
    public void EnableOverHeadSwingHitbox()
    {
        attackHitboxes[2].SetActive(true);
    }
    public void DisableOverHeadSwingHitbox()
    {
        attackHitboxes[2].SetActive(false);
    }
    #endregion

    //Phase 2
    #region Explode
    public void Explode()
    {
        Debug.Log("Im exploding");
        animator.SetBool("isExploding", true);
        isMoving = false;
        isAttacking = true;
    }
    public void ExplodeEnd()
    {
        hasExploded = true;
        attackCounter = 0;
        isAttacking = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isTaunting", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isSlashing", false);
        animator.SetBool("isWhirlwinding", false);
        animator.SetBool("isOvrSwinging", false);
        animator.SetBool("isExploding", false);
    }
    #endregion
    #region Charge
    public void Charge()
    {
        Debug.Log("Im charging");
        animator.SetBool("isCharging", true);
        age.speed = chargeSpeed;
        isMoving = true;
        isAttacking = true;
    }
    #endregion
    #region SlamAttack
    public void SlamAttack()
    {
        Debug.Log("Im slamming");
        animator.SetBool("isSlamming", true);
        isAttacking = true;
    }
    public void EnableSlamHitbox()
    {
        attackHitboxes[3].SetActive(true);
    }
    public void DIsableSlamHitbox()
    {
        attackHitboxes[3].SetActive(false);
    }
    #endregion
    #region Run2
    public void Run2()
    {
        Debug.Log("Im running");
        animator.SetBool("isRunning2", true);
        age.speed = movementSpeed;
        playerNearbyEndsAnimation = true;
        isMoving = true;
        isAttacking = true;
    }
    #endregion
    #region Punching
    public void Punching()
    {
        animator.SetBool("isPunching", true);
        isAttacking = true;
    }
    public void EnablePunchHitbox()
    {
        attackHitboxes[4].SetActive(true);
    }
    public void DIsablePunchHitbox()
    {
        attackHitboxes[4].SetActive(false);
    }
    #endregion
    #region Lazer
    public void LazerBeam()
    {
        Debug.Log("Im firin mah lazer");
        animator.SetBool("isFiringLazer", true);
        isAttacking = true;
    }
    public void SpawnLazer()
    {
        this.lb.useLaser = true;
    }
    public void DisableLazer()
    {
        this.lb.useLaser = false;
    }
    #endregion
    public void Death()
    {
        Debug.Log("Im Dying");
        animator.SetBool("isIdling2", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isRunning2", false);
        animator.SetBool("isFiringLazer", false);
        animator.SetBool("isPunching", false);
        animator.SetBool("isDying", true);
        isAttacking = true;
    }
    public void Idles()
    {
        animator.SetBool("isIdling", true);
        isAttacking = true;
    }
    public void EndChain()
    {
        attackCounter = 0;
        isAttacking = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isTaunting", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isSlashing", false);
        animator.SetBool("isWhirlwinding", false);
        animator.SetBool("isOvrSwinging", false);
        animator.SetBool("isIdling2", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isRunning2", false);
        animator.SetBool("isFiringLazer", false);
        animator.SetBool("isPunching", false);
    }
    public void endIdle()
    {
        animator.SetBool("isIdling", false);
        isAttacking = false;
    }
    public void Idle2()
    {
        animator.SetBool("isIdling2", true);
        isAttacking = true;
    }
    public void endIdle2()
    {
        attackCounter += 1;
        isAttacking = false;
        isMoving = false;
        animator.SetBool("isIdling2", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isSlamming", false);
        animator.SetBool("isRunning2", false);
        animator.SetBool("isPunching", false);
        animator.SetBool("isFiringLazer", false);
    }
    public void Idle()
    {
        attackCounter += 1;
        isAttacking = false;
        isMoving = false;
        animator.SetBool("isIdling", false);
        animator.SetBool("isTaunting", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isSlashing", false);
        animator.SetBool("isWhirlwinding", false);
        animator.SetBool("isOvrSwinging", false);
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

