using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerManger : MonoBehaviour
{
    // members
    [Header("Refrences")]
    private PlayerInput playerInput;
    private GameManager gameManager;
    public Animator animator;
    private AudioController audioController;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody playerBody;



    [Header("Player States")]
    public PlayerStates currentState;
    public bool inputsEnable;
    public bool playerInputsEnable;
    public bool stopMovementEvent;
    public bool isDashing = false;
    public bool isTalking = false;
    public bool isDying = false;

    [Header("Initial Start Position")]
    private Vector3 intialStartPos;

    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public GameObject playerUi;
    private bool isUICreated;
    [SerializeField] private Image Hud; 
    [SerializeField] private int numberOfHearts;
    private Image[] hearts; // the full array of hearts in the game
    //Hud
     private Sprite[] allHuds;
    //hearts
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Seeds")]
    public Seeds seeds;


    [Header("Attacking")]
    public float chargeTime;
    public bool chargedAttack = false;
    public bool isAttackAnimationActive;
    public int inputType;

    [Header("Audio Caller")]
    public AudioType playingAudio; // the currently playing audio
    [SerializeField] private AudioType queueAudio; // the next audio to play
    public AudioClip clip;
    public bool audioJobSent = false; // if job sent is true then it won't play
    private Dictionary<AudioType, AudioClip> ourAudio = new Dictionary<AudioType, AudioClip>();
    private List<AudioController.AudioObject> audioObjects = new List<AudioController.AudioObject>();


    #region Unity Functions
    private void Start()
    {
        currentHealth = maxHealth;
        numberOfHearts = currentHealth;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerBody = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        audioController = GetComponent<AudioController>();
    }
    private void Update()
    {
        if(playerInput.pause)
        {
            gameManager.gamePaused = (!gameManager.gamePaused);
            inputsEnable = !inputsEnable;            
        }
        
        if (inputsEnable == true)
        {
            if (playerMovement.IsGrounded())
            {
                if(playerInput.movementInput == Vector3.zero)
                {
                    //Debug.Log(playerInput.movementInput);
                }

                if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true && isAttackAnimationActive == false && isDashing == false)
                {
                    SetPlayerState(PlayerStates.Moving);
                }
                else if (playerInput.movementInput == Vector3.zero && playerMovement.IsGrounded() == true && isDashing == false && isAttackAnimationActive == false)
                    SetPlayerState(PlayerStates.Idle);
            }
            else
            {
                SetPlayerState(playerMovement.checkYVelocity(currentState));
            }


            if (playerInput.jumpInput && playerMovement.IsGrounded() == true)
            {
                playerMovement.InitateJump();
                animator.SetBool("isJumping", true);
            }

           if (playerInput.dash && playerMovement.isDashing == false)
            {
                animator.SetBool("isDashing", true);
                playerMovement.isDashing = true;
                StartCoroutine(playerMovement.Dash());
            }

            if (playerMovement.isDashing == true)
            {
                PlayerStates state;

                if (playerMovement.IsGrounded() == true)
                    state = PlayerStates.GroundedDash;
                else
                    state = PlayerStates.InAirDash;
                SetPlayerState(state);
            }


            if (playerMovement.IsGrounded() )
            {
                if (playerInput.attack)
                {
                    SetPlayerState(PlayerStates.PrimaryAttack);
                    isAttackAnimationActive = true;
                }

                if (playerInput.secondaryAttack && chargeTime <= 1f)
                {
                    SetPlayerState(PlayerStates.SecondaryAttack);
                    isAttackAnimationActive = true;
                }

                if (playerInput.chargedSecondaryAttack)
                {
                    chargeTime += Time.deltaTime;

                    animator.SetTrigger("Charging");
                    playerAttack.Rotate();

                    if (chargeTime > 1)
                    {
                        animator.SetInteger("Mouse Input", 2);
                        SetPlayerState(PlayerStates.ChargingAttack);
                        if (!chargedAttack)
                        {
                            chargedAttack = true;
                            animator.SetBool("Attacking", true);
                        }
                    }
                }

                if (!playerInput.chargedSecondaryAttack && chargeTime > 1f)
                {
                    animator.SetTrigger("LaunchChargedAttack");
                    animator.ResetTrigger("InputPressed");
                    animator.ResetTrigger("Charging");
                    chargedAttack = false;
                    isAttackAnimationActive = true;
                    SetPlayerState(PlayerStates.LaunchChargedAttack);
                }
            }
            else
            {

            }
        }

        // Current StateHandling 
        switch (currentState)
        {
            case (PlayerStates.Idle):
                animator.SetBool("isRunning", false);
                stopMovementEvent = false;
                break;
            case (PlayerStates.Moving):
                animator.SetBool("isRunning", true);
                break;
            case (PlayerStates.Jumping):
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.Falling):
                animator.SetBool("isFalling", true);
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.GroundedDash):
                SetPlayerState(currentState);
                stopMovementEvent = true;
                break;
            case (PlayerStates.FallingAndMoving):
                animator.SetBool("isFalling", true);
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.JumpingAndMoving):
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.DirectionalAttack):
                isAttackAnimationActive = true;
                break;
        }


        #region Handeling Player Health 
        animator.SetInteger("Health", currentHealth);
        if (isUICreated == true)
            VisualizeHealth();
        if (currentHealth <= 0)
        {
            SetPlayerState(PlayerStates.Dying);
            isDying = true;
        }
        
        
        #endregion
    }
    #endregion


    public void SetPlayerState(PlayerStates newState)
    {
        if(newState != currentState)
        {
            //On Leave from previous State
            switch (currentState)
            {
                case PlayerStates.InAirDash:
                    if(playerMovement.isDashing == false)
                    {
                      
                    }
                    break;
            }
            currentState = newState;
              
            //On Enter
            switch(currentState)
            {
                case PlayerStates.PrimaryAttack:
                    inputType = 0;
                    playerAttack.LaunchAttack(inputType);
                    break;
                case PlayerStates.SecondaryAttack:
                    inputType = 1;
                    chargeTime = 0;
                    playerAttack.timeOfCharge = 0;
                    playerAttack.chargedAttackMultiplier = 1.4f;
                    playerAttack.LaunchAttack(inputType);
                    break;
                case PlayerStates.LaunchChargedAttack:
                    inputType = 2;
                    chargedAttack = false;
                    playerAttack.LaunchAttack(inputType);
                    chargeTime = 0;
                    playerAttack.timeOfCharge = 0;
                    playerAttack.chargedAttackMultiplier = 1.4f;
                    break;
                case PlayerStates.Dying:
                    stopMovementEvent = true;
                    break;
                case PlayerStates.Landing:
                    playerBody.velocity = Vector3.zero;
                    break;
            }
        }
    }

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
            int numberOfRandomNumbers =4; // Number of random numbers to generate
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
        else if(type == "ChargedAttack")
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
        else if(type == "Damaged")
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

    #region Disable All Inputs
    public void DisableAllInputs()
    {
        playerInput.enabled = false;
    }
    #endregion 



    public void CreateHealthBar()
    {
        playerUi.SetActive(true);
        //Set up Health Bar
        if (playerUi != null)
        {
            var childrenList = new List<Image>();
   
            // the string name needs to be exact for the function to work
            Transform root = playerUi.transform;
            Hud = GetChildByName(root, "Hud").GetComponent<Image>();
            Transform heartHolder = GetChildByName(root, "Heart Holder");

            if (heartHolder != null)
            {
                foreach (Transform child in heartHolder.transform)
                {
                    Image i = child.GetComponent<Image>();
                    childrenList.Add(i);
                }
                hearts = childrenList.ToArray();
                childrenList.Clear();
                isUICreated = true;
            }
        }
        else
        {
            Debug.Log("Player Ui is null");
            return;
        }
       
        
    }

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
    #endregion


    Transform GetChildByName(Transform parent, string name)
    {
        // Base Case: If the parent has no children, return null
        if (parent.childCount == 0)
        {
            return null;
        }

        // Check each child of the parent
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // If the child has the desired name, return it
            if (child.name == name)
            {
                return child;
            }

            // If not, recursively search for the child in the children of the child
            Transform result = GetChildByName(child, name);

            // If the child is found in the recursive call, return it
            if (result != null)
            {
                return result;
            }
        }

        // If no child with the desired name is found, return null
        Debug.Log("name must be wrong");
        return null;
    }


    void VisualizeHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            //This is for creating our final health bar, change number of hearts to make amount visible in game 
            if (i < numberOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;

            //Handling visually representing players health in realtion to number of hearts  
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
        // Saftey net check making sure our hearts equal the current health
        if (numberOfHearts < currentHealth)
        {
            numberOfHearts = currentHealth;
        }
    }
}
public enum PlayerStates
{
    None,
    Idle,
    Moving,
    Landing,
    Jumping,
    JumpingAndMoving,
    GroundedDash,
    InAirDash,
    Falling,
    FallingAndMoving,
    Dying,

    //Regular attacks 
    PrimaryAttack,
    SecondaryAttack,
    ChargingAttack,
    LaunchChargedAttack,
    DirectionalAttack,


    //Seed Abilities

    //Status Effects 
    Stunned,
}


