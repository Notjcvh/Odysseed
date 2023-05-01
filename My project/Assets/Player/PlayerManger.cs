using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;


public class PlayerManger : MonoBehaviour
{


    // members
    [Header("Refrences")]
    private GameManager gameManager;
    private PlayerInput playerInput;
    private CameraController cam;
    public Animator animator;
    private AudioController audioController;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    public Rigidbody playerBody;
    public SphereCollider sphereCollider;
    private PlayerBlock playerBlock;
    private PlayerUI playerUI;

    [Header("Initial Start Position")]
    private Vector3 intialStartPos;

    [Header("Player States")]
    public SuperStates superStates;
    public SubStates subStates;

    public bool activeInputsEnabled;
    public bool inactiveInputsEnabled; 
    [SerializeField] private bool stopMovementEvent;
    [SerializeField] private bool isUICreated = false;
    [SerializeField] private bool isDashing = false;
    public bool isTalking = false;
    [SerializeField] private bool isGrounded = false;
    public bool isDying = false;
    public bool isAttacking = false;
    public bool blocking = false;

    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Movement")]
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float targetSpeed;

    [Header("GroundCheck")]
    public LayerMask Ground;
    public float distanceToGround;
    public float lastPlayerPosY;
    [SerializeField] private float jumpForce;
    private float gravity;
    public AnimationCurve gravityValueCurve;
    public float gravityMultiplier = 0;
    private IEnumerator gravityCorutine;

    [Header("Dash")]
    public float force;
    private IEnumerator dashCorutine;

    [Header("Seeds")]
    public Seeds seeds;

    [Header("Attacking")]
    public float chargeTime;
    public bool chargedAttack = false;
    public bool isAttackAnimationActive;
    public float timeOfCharge;
    private int count = 0;
    public float chargedAttackMultiplier = 1.4f;

    [Header("Blocking")]
    private float maxBlockStamina = 100;
    private float _currentBlockStamina;

    [Header("Audio Caller")]
    public AudioType playingAudio; // the currently playing audio
    [SerializeField] private AudioType queueAudio; // the next audio to play
    public AudioClip clip;
    public bool audioJobSent = false; // if job sent is true then it won't play
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();

    //Getters and Setters
    public int PlayerHealth { get { return currentHealth; } }
    public float PlayerBlockHealth { get { return _currentBlockStamina; } set { _currentBlockStamina = value; } }
    public Vector3 DirectionInput {get { return playerInput.movementInput; }}
    public Vector3 MovementVector { get { return movementVector; } set { movementVector = value; } }
    public Quaternion TargetRot { get { return targetRotation; } set { targetRotation = value; } }
    public float TargetSpeed { get { return targetSpeed; } }
    public float JumpForce { get { return jumpForce; } }
    public CameraController CameraControl { get { return cam; } }
    public bool UiCreated { get { return isUICreated; } set { isUICreated = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public bool StopMovement { get { return stopMovementEvent; } set{ stopMovementEvent = value; } }

    #region Unity Functions
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerBody = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        audioController = GetComponent<AudioController>();
        sphereCollider = GetComponent<SphereCollider>();
        playerBlock = GetComponent<PlayerBlock>();
        playerUI = GetComponent<PlayerUI>();
        currentHealth = maxHealth;
        _currentBlockStamina = maxBlockStamina;
        animator.SetInteger("Health", currentHealth);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Blocked(playerBody, Random.Range(10, 50));
        }


        Debug.Log(activeInputsEnabled);

        //   Debug.Log("current block health " + PlayerBlockHealth);

        if (IsGrounded() == true)
        {
            SetSuperState(SuperStates.Grounded);
        }
        else
        {
            //Check Y velocity to see if we are Rising or Falling
            SetSuperState(checkYVelocity(superStates));
            animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
        }
        //StateHandling
        switch (superStates)
        {
            case SuperStates.Falling:
                animator.SetBool("isGrounded", IsGrounded());
                break;
            case SuperStates.Rising:
                animator.SetBool("isGrounded", IsGrounded());
                break;
            case SuperStates.Grounded:
                break;
        }
        if (activeInputsEnabled == true)
        {
            HandleInputs();
            switch (subStates)
            {
                case (SubStates.ChargingAttack):
                    animator.SetTrigger("Charging");
                    animator.SetFloat("ChargeTime", chargeTime);
                    timeOfCharge += Time.deltaTime;
                    float intervalDuration = 0.25f;
                    if (timeOfCharge >= intervalDuration && chargeTime <= 2)
                    {
                        chargedAttackMultiplier += 0.2f;
                        timeOfCharge = 0;
                        count += 1;
                    }

                    if (chargeTime > 1.98f && chargeTime < 2 && count < 4)
                    {
                        count += 1;
                        chargedAttackMultiplier += 0.2f;
                    }
                break;
                case (SubStates.Guarding):
                    animator.SetBool("Guarding", true);
                    break;
            }

            #region Handeling Player Health 
           
            if (isUICreated == true)
            {
                playerUI.VisualizeHealth();
                animator.SetInteger("Health", currentHealth);
            }
            if (currentHealth <= 0)
            {
                SetSuperState(SuperStates.Dying);
            }
            #endregion
        }
    }

    #endregion
    public void SetSuperState(SuperStates newState)
    {
        if (newState != superStates)
        {
            //On Leave from previous State
            switch (superStates)
            {
                case SuperStates.Grounded:
                    break;
                case SuperStates.Falling:
                    playerBody.useGravity = true;
                    if(isDashing == true)
                    {
                        // Stop applying force to the rigidbody
                        playerBody.velocity = Vector3.zero;
                        isDashing = false;
                        stopMovementEvent = false;
                    }
                    if(isAttacking == true)
                    {
                        animator.SetBool("Attacking", false);
                        StartCoroutine(playerMovement.ApplyGravity());
                        playerBody.useGravity = true;
                        isAttacking = false;
                        stopMovementEvent = false;
                    }
                    break;
                case SuperStates.Rising:
                  
                    break;
            }
            superStates = newState;
            //On Enter
            switch (superStates)
            {
                case SuperStates.Grounded:

                    animator.SetFloat("PlayerYVelocity", 0);
                    animator.SetBool("isGrounded", IsGrounded());
                    animator.SetBool("isFalling", false);
                    break;
                case SuperStates.Falling:
                    animator.SetBool("isFalling", true);
                    animator.SetBool("isRunning", false);
                    break;
                case SuperStates.Rising:

                    animator.SetBool("isRunning", false);
                    playerAttack.ResetCombo();
                    break;
                case SuperStates.Dying:
                    animator.SetTrigger("Death");
                    break;
            }
        }
    }
    public void SetSubState(SubStates newState)
    {
        if (newState != subStates)
        {
            //On Leave from previous State
            switch (subStates)
            {
                case SubStates.Idle:
                    break;
                case SubStates.Moving:
                    break;
                case SubStates.Attacking:
                    break;
                case SubStates.ChargingAttack:
                    animator.ResetTrigger("Charging");
                    break;
                case SubStates.Dashing:
                    break;
                case SubStates.Guarding:
                    animator.SetBool("Guarding", false);
                    animator.ResetTrigger("StartGuard");
                    stopMovementEvent = false;
                    break;
            }
            subStates = newState;
            //On Enter
            switch (subStates)
            {
                case SubStates.Idle:
                    animator.SetBool("isRunning", false);
                    break;
                case SubStates.Moving:
                    break;
                case SubStates.Attacking:
                    isAttacking = true;
                    stopMovementEvent = true;
                    chargeTime = 0;
                    animator.SetFloat("ChargeTime", chargeTime);
                    break;
                case SubStates.ChargingAttack:
                    animator.SetBool("Attacking", true);
                    break;
                case SubStates.RunningJump:
                    animator.SetBool("isJumping", true);
                    //playerMovement.InitateJump();
                    break;
                case SubStates.Jumping:
                    animator.SetBool("isJumping", true);
                    break;
                case SubStates.Dashing:
                    isDashing = true;
                    animator.SetBool("isDashing", true);
                    break;
                case SubStates.LaunchChargedAttack:
                    playerAttack.LaunchAttack(2);
                    animator.SetTrigger("LaunchChargedAttack");
                    chargeTime = 0;
                    count = 0;
                    chargedAttackMultiplier = 0;
                    animator.SetFloat("ChargeTime", chargeTime);
                    break;
                case SubStates.Guarding:
                    stopMovementEvent = true;
                    animator.SetTrigger("StartGuard");
                    animator.SetBool("Guarding", true);
                    break;
                case SubStates.SeedAbilityAttack:
                    stopMovementEvent = true;
                    isAttacking = true;
                    playerAttack.LaunchAttack(3);
                    break;
            }
        }
    }
    void HandleInputs()
    {
        if (superStates == SuperStates.Grounded)
        {
            if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true && !isAttacking)
            {
                SetSubState(SubStates.Moving);
                animator.SetBool("isRunning", true);
            }
            else if (playerInput.attack && !isAttacking)
            {
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(0);
            }
            else if (playerInput.secondaryAttack && chargeTime < .5f && !isAttacking)
            {
                SetSubState(SubStates.Attacking);
                animator.SetBool("isRunning", false);
                playerAttack.LaunchAttack(1);
            }
            else if (playerInput.chargedSecondaryAttack && !isAttacking)
            {
                chargeTime += Time.deltaTime;
                animator.SetFloat("ChargeTime", chargeTime);
                SetSubState(SubStates.ChargingAttack);
            }
            else if (!playerInput.chargedSecondaryAttack && subStates == SubStates.ChargingAttack)
            {
                SetSubState(SubStates.LaunchChargedAttack);
                isAttacking = true;
            }
            else if (playerInput.jumpInput && playerInput.movementInput == Vector3.zero && playerBody.velocity.y >= 0)
            {
                SetSubState(SubStates.Jumping);
            }
            else if (playerInput.dash && isDashing == false)
            {
                Debug.Log("Start Dash");
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Grounded);
                SetSubState(SubStates.Dashing);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                SetSubState(SubStates.Guarding);
            }
            else if(!isDashing && !isAttacking)
            {
                SetSubState(SubStates.Idle);
                animator.SetBool("isRunning", false);
            }

            //for dashing and jumping we need the initial 
            //combined inputs
            if (playerInput.movementInput != Vector3.zero && playerInput.jumpInput && stopMovementEvent != true)
            {
                SetSubState(SubStates.RunningJump);
            }

            if (playerInput.movementInput != Vector3.zero && playerInput.dash && isDashing == false)
            {
                animator.SetBool("isRunning", false);
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Grounded);
                SetSubState(SubStates.Dashing);
            }

            //Moving and Attacking 
            if (playerInput.attack && playerInput.movementInput != Vector3.zero && isAttacking == false)
            {
              //  Debug.Log("Change Direction");
                animator.SetBool("isRunning", false);
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(0);
            }

            if(playerInput.block && playerInput.movementInput != Vector3.zero)
            {
                SetSubState(SubStates.Guarding);
            }
        }

        #region Rising and Falling SuperStates
        else if (superStates == SuperStates.Falling)
        {
            if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true && !isAttacking)
            {
                SetSubState(SubStates.Moving);
            }
            else if (playerInput.attack && !isAttacking)
            {
                SetSubState(SubStates.Attacking);
                isAttacking = true;
                playerAttack.LaunchAttack(0);
            }
          /*  else if (playerInput.secondaryAttack && chargeTime <= 1f && !isAttacking)
            {
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(1);
            }*/
            else if (playerInput.dash && isDashing == false)
            {
                Debug.Log("Start Dash");
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Falling);
                SetSubState(SubStates.Dashing);
            }
            else if (!isDashing && !isAttacking)
            {
                SetSubState(SubStates.Idle);
            }

            if (playerInput.movementInput != Vector3.zero && playerInput.dash && isDashing == false)
            {
                Debug.Log("Start Dash");
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Grounded);
                SetSubState(SubStates.Dashing);
            }

            if (playerInput.attack && playerInput.movementInput != Vector3.zero && isAttacking == false)
            {
                isAttacking = true;
                stopMovementEvent = true;
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(0);
            }
        }
        else if (superStates == SuperStates.Rising)
        {
            if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true)
            {
                SetSubState(SubStates.Moving);
            }
            else if (playerInput.attack && !isAttacking)
            {
                SetSubState(SubStates.Attacking);
                isAttacking = true;
                playerAttack.LaunchAttack(0);
            }
         /*   else if (playerInput.secondaryAttack && chargeTime <= 1f && isAttackAnimationActive == false)
            {
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(1);
            }*/
           
            else if (playerInput.dash && isDashing == false)
            {
                Debug.Log("Start Dash");
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Rising);
                SetSubState(SubStates.Dashing);
            }
            else if (!isDashing && !isAttacking)
            {
                SetSubState(SubStates.Idle);
            }
            //Moving and Dashing 
            if (playerInput.movementInput != Vector3.zero && playerInput.dash && isDashing == false)
            {
                stopMovementEvent = true;
                playerMovement.CreateDash(SuperStates.Grounded);
                SetSubState(SubStates.Dashing);
            }

            //Moving and Attacking 
            if (playerInput.attack && playerInput.movementInput != Vector3.zero && isAttacking == false)
            {
                stopMovementEvent = true;
                isAttacking = true;
                SetSubState(SubStates.Attacking);
                playerAttack.LaunchAttack(0);
            }
        }
        #endregion
    }
    public void Blocked(Rigidbody attacker, int damage)
    {
        //Activate Behavior
        playerBlock.HitSomething(attacker);

        if (_currentBlockStamina > 0)
        {
            blocking = true;
            //Deal damage to health bar 
            // Ensure that block stamina does not go below zero
            _currentBlockStamina = Mathf.Max(_currentBlockStamina - damage, 0);
            float blockDeductionPercentage = _currentBlockStamina / maxBlockStamina;
            StartCoroutine(playerUI.ReduceStamina(blockDeductionPercentage));
        }
    }



  #region Ground Check
        public bool IsGrounded()
        {
            bool isHit;
            Vector3 direction = Vector3.down;
            RaycastHit hit;
            float distanceCheck = sphereCollider.bounds.extents.y + distanceToGround;
            //  Debug.DrawRay(collider.bounds.center, Vector3.down * distanceCheck);

            Ray ray = new Ray(sphereCollider.bounds.center, Vector3.down * distanceToGround);

            if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out hit, distanceToGround, Ground, QueryTriggerInteraction.Ignore))
            {
                isHit = true;
                Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Ray ray2 = new Ray(hitPos, hit.normal * distanceToGround);
                float angle = Mathf.Asin(Vector3.Cross(ray.direction, ray2.direction).magnitude) * Mathf.Rad2Deg;
                if (angle > 0 && angle <= 25)
                {
                   // Debug.Log("We are on a walkable slope");
                    //  this.transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 1)

                }

                return isHit;
            }
            else
            {
                isHit = false;
                return isHit;
            }
        }

        //Checking the change in the position of the player 
        public SuperStates checkYVelocity(SuperStates currentState)
        {
            if (transform.hasChanged)
            {
                if (transform.position.y > lastPlayerPosY)
                {
                    currentState = SuperStates.Rising;
                }
                else if (transform.position.y < lastPlayerPosY)
                {
                    currentState = SuperStates.Falling;
                    //  isFalling = true;
                }
            }
            lastPlayerPosY = transform.position.y;
            return currentState;
        }


        //Calling an animation envent to stop anim form looping 
        public void FallingAnimationEnded()
        {
            animator.SetBool("isFalling", false);
        }
    #endregion


    #region Animation Event Calls
    public void StartJump()
    {
        playerMovement.InitateJump();
    }
    public void JumpAnimationEnded()
    {
        animator.SetBool("isJumping", false);
        playerBody.useGravity = false;
        StartCoroutine(playerMovement.ApplyGravity());
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z);
    }

    public void DashAnimationEnded()
    {
        animator.SetBool("isDashing", false);
    }

    public void CanRotate()
    {
        playerAttack.canRotate = false;
    }


    public void EndWindUp()
    {
        animator.SetBool("Attacking", false);
        Debug.Log("Called");
    }

    #endregion

    #region Called form Other Scripts 
    public void DisableAllInputs()
    {
        playerInput.enabled = false;
    }

    //resets the variables to their initial values to ensure
    // that they are not carrying over any data when we switch states
    public void SetInputstToInactive()
    {
        playerInput.ResetActiveInputs();
    }
    public void SetInputsToActive()
    {
        playerInput.ResetPassiveInputs();    
    }
    #endregion





    #region Player Restore Health or Taking Damgage
    public void TakeDamage(int damage)
     {
         currentHealth -= damage;
         SelectAudio("Damage");
     }
    public void RestoreHealth()
    {
       currentHealth = maxHealth;
    }

    public void CallPlayerUi()
    {
        playerUI = GetComponent<PlayerUI>();
        //Call whatever functions the player Ui needs to call
        playerUI.CreateHealthBar();
    }
    #endregion


    #region Sound looping

    public void SelectAudio(string type)
    {
        //PlayerAttack
        //ChargedAttack
        if (type == "Walk")
        {
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 4; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
                switch (randomNumber)
                {
                    case (1):
                        sendingAudio = AudioType.PlayerWalk1;
                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerWalk2;
                        break;
                    case (3):
                        sendingAudio = AudioType.PlayerWalk3;
                        break;
                    case (4):
                        sendingAudio = AudioType.PlayerWalk4;
                        break;
                    case (5):
                        sendingAudio = AudioType.PlayerWalk5;
                        break;
                    default:
                        break;
                }
            }
            ManageAudio(sendingAudio);
        }
        else if (type == "Death")
        {
            AudioType sendingAudio = AudioType.None;
            sendingAudio = AudioType.PlayerDeath;
            ManageAudio(sendingAudio);
        }
        else if (type == "Attack")
        {
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 4; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                switch (randomNumber)
                {
                    case (1):
                        sendingAudio = AudioType.PlayerAttack1;
                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerAttack2;
                        break;
                    case (3):
                        sendingAudio = AudioType.PlayerAttack3;
                        break;
                    case (4):
                        sendingAudio = AudioType.PlayerAttack4;
                        break;
                    case (5):
                        sendingAudio = AudioType.PlayerAttack5;
                        break;
                    default:
                        break;
                }
            }
            ManageAudio(sendingAudio);
        }
        else if (type == "ChargedAttack")
        {
            AudioType sendingAudio = AudioType.PlayerChargedAttack1;
            /*
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 1; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                switch (randomNumber)
                {
                    case (1):

                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerChargedAttack2;
                        break;
                    default:
                        break;
                }
            }*/
            ManageAudio(sendingAudio);

        }
        else if (type == "Damaged")
        {
            AudioType sendingAudio = AudioType.None;
            int numberOfRandomNumbers = 4; // Number of random numbers to generate
            int minRange = 1; // Minimum value for random numbers
            int maxRange = 5;
            int randomNumber = Random.Range(minRange, maxRange + 1); // Generate a random number within the specified range
            for (int i = 0; i < numberOfRandomNumbers; i++)
            {
                switch (randomNumber)
                {
                    case (1):
                        sendingAudio = AudioType.PlayerDamaged1;
                        break;
                    case (2):
                        sendingAudio = AudioType.PlayerDamaged2;
                        break;
                    case (3):
                        sendingAudio = AudioType.PlayerDamaged3;
                        break;
                    case (4):
                        sendingAudio = AudioType.PlayerDamaged4;
                        break;
                    default:
                        break;
                }
            }
            ManageAudio(sendingAudio);
        }
        else
        {
            return;
        }
    }
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
    IEnumerator WaitToPlay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        audioJobSent = false;
    }
    #endregion

    void OnDrawGizmos()
    {

        /*
         float ydirection = playerBody.velocity.y;
         if(ydirection > 0)
         {
             Gizmos.color = Color.red;
         }
         else if(ydirection < 0)
         {
             Gizmos.color = Color.green;
         }
         Gizmos.DrawRay(sphereCollider.bounds.center, new Vector3(0, ydirection, 0).normalized * 10f);












        // Set the color of the gizmo
         Gizmos.color = Color.yellow;

         // Draw the box cast using Gizmos.DrawWireCube and Gizmos.DrawRay
         Gizmos.DrawWireCube(sphereCollider.bounds.center, sphereCollider.bounds.size);
         Gizmos.DrawRay(sphereCollider.bounds.center, Vector3.down * distanceToGround);

         Ray ray = new Ray(sphereCollider.bounds.center, Vector3.down * distanceToGround);

         // Check if the box cast hits anything
         RaycastHit hit;
         if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out hit, distanceToGround, Ground, QueryTriggerInteraction.Ignore))
         {
             Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
             // Set the color of the gizmo to red if there is a hit
             Gizmos.color = Color.red;
             // Draw a sphere at the hit point using Gizmos.DrawSphere
             Gizmos.DrawSphere(hitPos, 0.1f);


             Gizmos.color = Color.green;
             Gizmos.DrawRay(hitPos, hit.normal * distanceToGround);
             Ray ray2 = new Ray(hitPos, hit.normal * distanceToGround);

             float angle = Mathf.Asin(Vector3.Cross(ray.direction, ray2.direction).magnitude) * Mathf.Rad2Deg;

             Gizmos.color = Color.blue;
             Vector3 rotationAxis = Vector3.Cross(Vector3.up, hit.normal);
             rotationAxis.z = 0;
             rotationAxis.x = 0;
             Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);

             Vector3 rotatedDirection = rotation * Vector3.forward;
             Gizmos.DrawRay(hitPos, rotatedDirection * distanceToGround);
         }




         // Draw a ray to visualize the player's movement direction
         Gizmos.color = Color.white;
         Gizmos.DrawRay(transform.position, MovementVector);

         // Cast a ray in front of the player to check for collisions
         RaycastHit hit2;
         float speed = MovementVector != Vector3.zero ? TargetSpeed : 0;
         if (Physics.Raycast(transform.position, MovementVector, out hit2, speed * Time.fixedDeltaTime))
         {
             // If the ray hits something, draw a red line up to the point of collision
             Gizmos.color = Color.red;
             Gizmos.DrawRay(transform.position, hit2.point - transform.position);
         }
         else
         {
             // If the ray doesn't hit anything, draw a green line up to the end of the movement vector
             Gizmos.color = Color.green;
             Gizmos.DrawRay(transform.position, MovementVector * Time.fixedDeltaTime * speed);
         }*/
    }

}
public enum SuperStates
{
    Falling,
    Grounded, 
    Rising, 
    Dying,
    Stunned
}
public enum SubStates
{
    None,
    Moving,
    Idle, 
    Dashing,
    Jumping,
    Attacking,
    Guarding,
    ChargingAttack,
    LaunchChargedAttack,
    RunningJump,
    SeedAbilityAttack,

}
