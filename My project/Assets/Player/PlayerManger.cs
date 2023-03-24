using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerManger : MonoBehaviour
{
    // members
    [Header("Refrences")]
    private PlayerInput playerInput;
    private GameManager gameManager;
    public Animator animator;


    [Header("Player States")]
    public PlayerStates currentState;
    public bool inputsEnable;

    [Header("Initial Start Position")]
    private Vector3 intialStartPos;


    public int maxHealth = 5;
    public int currentHealth;

    [Header("Hearts Images")] //Heart Sprites
    public int numberOfHearts;
    public Sprite[] hearts; // the full array of hearts in the game
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Seeds")]
    public Seeds seeds;
    
    #region Unity Functions
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();

        transform.position = gameManager.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (playerInput.enabled == true)
                DisableAllInputs();
            else
            {
                playerInput.enabled = true;
            }
        }
        inputsEnable = playerInput.enabled;
        //if (playerInput.changeWeaponState) // calls the ChangeWeaponState Method
        //  ChangeWeaponState(element);
        if (currentHealth > numberOfHearts)  // Making sure our health equals the number of hearts 
            numberOfHearts = currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) //Handling visually representing players health in realtion to number of hearts  
                hearts[i] = fullHeart;
            else
                hearts[i] = emptyHeart;

        //    if (i < numberOfHearts)  //This is for creating our final health bar, change number of hearts to make amount visible in game 
          //      hearts[i].enabled= true;
           // else
             //   hearts[i].enabled = false;
        }

        if (Input.GetKey(KeyCode.N))
        {
            gameManager.playerHasDied = true;
            Destroy(this.gameObject);
            currentHealth = maxHealth;
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Health Restored");
    }

    public void DisableAllInputs()
    {
        playerInput.enabled = false;
    }
}

public enum PlayerStates
{
    
    Idle,
    Attacking,
    Dashing,
    Moving,
    Jumping, 
    Falling,
    Dying,
}


