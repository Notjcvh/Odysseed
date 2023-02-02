using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    // members
    [Header("Refrences")]
    public PlayerStats stats;    //Handling Functions related to the player stats scriptable obj
    private PlayerInput playerInput;
    public ElementType element;
    private GameManager gameManager;
    public Animator animator;

    public int currentHealth;

    [Header("Hearts Images")] //Heart Sprites
    public int numberOfHearts;
    public Image[] hearts; // the full array of hearts in the game
    public Sprite fullHeart;
    public Sprite emptyHeart;


    [Header("Shader Settings")] // controls the players outline
    public Color color;
    public Material material;
    public GameObject playerOutline;

    #region Unity Functions
    private void Awake()
    {
        currentHealth = stats.health;
    }
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        
        animator = GetComponent<Animator>();

        if(Input.GetKeyDown(KeyCode.F9))
        {
            transform.position = gameManager.position;
        }
        playerInput = GetComponent<PlayerInput>();
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
            currentHealth = stats.health;
        }
    }
    #endregion

    #region Private Functions
    private void ChangeWeaponState(ElementType state)
    {

        if (state == ElementType.Base)
        {
            print(state);
            element = ElementType.Water;
            color = Color.blue;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
        if (state == ElementType.Water)
        {
            element = ElementType.Fire;
            color = Color.red;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
        if (state == ElementType.Fire)
        {
            element = ElementType.Earth;
            color = Color.green;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
        if (state == ElementType.Earth)
        {
            print(state);
            element = ElementType.Base;
            color = Color.black;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}



public enum ElementType
{
    Base = 0,
    Water = 1,
    Fire = 2,
    Earth = 3,

}


