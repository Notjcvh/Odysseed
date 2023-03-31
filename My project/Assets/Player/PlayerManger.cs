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

    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")] //Heart Sprites
    public int numberOfHearts;
    public Image[] hearts; // the full array of hearts in the game
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Seeds")]
    public Seeds seeds;
    
    #region Unity Functions
    private void Start()
    {
        currentHealth = maxHealth;
        numberOfHearts = currentHealth;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            inputsEnable = true;
        }



        if (inputsEnable == true)
            playerInput.enabled = true;
        else
            playerInput.enabled = false;
      
        //Handeling Player Health 
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

        if (currentHealth <= 0)
        {
            gameManager.PlayerHasDied();
            Destroy(this.gameObject);
        }
    }
    #endregion

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
        Debug.Log("Health Restored");
    }
    #endregion

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


