using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    [SerializeField] private Rigidbody playerBody;

    [Header("Player States")]
    public PlayerStates currentState;
    public bool inputsEnable;
    public bool playerInputsEnable;

    [Header("Initial Start Position")]
    private Vector3 intialStartPos;

    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public GameObject playerUi;
    [SerializeField] private Image Hud; 
    [SerializeField] private int numberOfHearts;
    public Image[] hearts; // the full array of hearts in the game

    //Hud
    [SerializeField] private Sprite[] allHuds;
    //hearts
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Seeds")]
    public Seeds seeds;

    public bool stopMovementEvent;
    public bool isDashing = false;
    public bool isTalking = false;
    #region Unity Functions

    [Header("Attacking")]
    public float chargeTime;
    public bool chargedAttack = false;
    public bool isAttackAnimationActive;
    public int inputType;

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


        //Set up Health Bar
        if (playerUi == null)
        {
        
            var childrenList = new List<Image>();
            playerUi = GameObject.FindGameObjectWithTag("PlayerUI");

            // the string name needs to be exact for the function to work
            Transform root = playerUi.transform;
            Hud = GetChildByName(root, "Hud").GetComponent<Image>();
            Transform heartHolder = GetChildByName(root, "Heart Holder");

            if(heartHolder != null)
            {
                foreach (Transform child in heartHolder.transform)
                {
                    Image i = child.GetComponent<Image>();
                    childrenList.Add(i);
                    hearts = childrenList.ToArray();
                }
            }
        }
    }
    private void Update()
    {
        if(playerInput.pause)
        {
            gameManager.gamePaused = (!gameManager.gamePaused);
            inputsEnable = !inputsEnable;            
        }
        
        if(inputsEnable == true)
        {
            if (playerMovement.IsGrounded())
            {
                if (playerInput.movementInput != Vector3.zero && stopMovementEvent != true && isAttackAnimationActive == false)
                    SetPlayerState(PlayerStates.Moving);
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

            if (playerInput.dash && isDashing == false)
            {
                isDashing = true;
                animator.SetBool("isDashing", true);
            }

            if (isDashing == true)
            {
                currentState = PlayerStates.Dashing;
            }


            //Attacking 
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
          
                if(chargeTime > 1)
                {
                    animator.SetInteger("Mouse Input", 2);
                    SetPlayerState(PlayerStates.ChargingAttack);
                    if(!chargedAttack)
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


     

        // Current StateHandling 
        switch (currentState)
        {
            case (PlayerStates.Idle):
                animator.SetBool("isRunning", false);
                break;
            case (PlayerStates.Moving):
                animator.SetBool("isRunning", true);
                audioController.PlayAudio(AudioType.PlayerWalk, false, 0, false);
                break;
            case (PlayerStates.Jumping):
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.Falling):
                animator.SetBool("isFalling", true);
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.Dashing):
                SetPlayerState(currentState);
                break;
            case (PlayerStates.FallingAndMoving):
                animator.SetBool("isFalling", true);
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
            case (PlayerStates.JumpingAndMoving):
                animator.SetFloat("PlayerYVelocity", playerBody.velocity.y);
                break;
        }


        #region Handeling Player Health 

        VisualizeHealth();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentHealth = 0;
        }


        if (currentHealth <= 0)
        {
            SetPlayerState(PlayerStates.Dying);
            gameManager.PlayerHasDied();
            Destroy(this.gameObject);
        }
        #endregion
    }
    #endregion


    void SetPlayerState(PlayerStates newState)
    {
        if(newState != currentState)
        {
            //On Leave
            switch (currentState)
            {
                //call that here
                case PlayerStates.Jumping:
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
                    animator.SetTrigger("Death");
                    break;

            }
        }
    }









    #region Disable All Inputs
    public void DisableAllInputs()
    {
        playerInput.enabled = false;
    }
    #endregion 

    #region Player Restore Health or Taking Damgage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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
    Dashing,
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


