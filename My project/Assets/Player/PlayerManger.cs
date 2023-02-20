using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    // members
    [Header("Refrences")]
    private PlayerInput playerInput;
    private GameManager gameManager;
    public Animator animator;

    [Header("Initial Start Position")]
    private Vector3 intialStartPos;


    public int maxHealth = 5;
    public int currentHealth;

    [Header("Hearts Images")] //Heart Sprites
    public int numberOfHearts;
    public Image[] hearts; // the full array of hearts in the game
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
        //gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();

        //Debugging stuff
        if (Input.GetKeyDown(KeyCode.F9))
        {
            transform.position = gameManager.position;
        }

     
    }

    private void Update()
    {
        //if (playerInput.changeWeaponState) // calls the ChangeWeaponState Method
          //  ChangeWeaponState(element);

        if (currentHealth > numberOfHearts)  // Making sure our health equals the number of hearts 
            numberOfHearts = currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) //Handling visually representing players health in realtion to number of hearts  
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            if (i < numberOfHearts)  //This is for creating our final health bar, change number of hearts to make amount visible in game 
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }

        if (currentHealth <= 0)
        {
            gameManager.PlayerHasDied();
            gameManager.ReloadPosition();
            Destroy(this.gameObject);
            currentHealth = maxHealth;
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }


   

}

