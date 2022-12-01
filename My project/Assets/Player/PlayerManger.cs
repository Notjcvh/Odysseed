using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    // members
    [Header("Refrences")]
    public PlayerStats stats;    //Handling Functions related to the player stats scriptable obj
    private PlayerInput playerInput; 
    public ElementType element;
    public VectorValue startingPosition; // get starting postion from GameManager
    private GameManager gameManager;
   

    [Header("Hearts Images")]     //Heart Sprites
    public Image[] hearts; // the full array of hearts in the game
    public Sprite fullHeart;
    public Sprite emptyHeart;


    [Header("Shader Settings")] // controls the players outline
    public Color color;
    public Material material;
    public GameObject playerOutline;

    #region Unity Functions
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        if (gameManager.hasDiedOnce == false)
        {
            transform.position = startingPosition.initialStartValue;
        }
        else
        {
            transform.position = gameManager.lastCheckPointPos;
        }

        playerInput = GetComponent<PlayerInput>();
        playerOutline = this.gameObject.transform.Find("Capsule/Capsule Outline").gameObject;
        material = playerOutline.GetComponent<Renderer>().material;
    }

    private void Update()
    { 
        if(playerInput.changeWeaponState) // calls the ChangeWeaponState Method
           ChangeWeaponState(element);       

        if(stats.health > stats.numberOfHearts)  // Making sure our health equals the number of hearts 
            stats.health = stats.numberOfHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < stats.health) //Handling visually representing players health in realtion to number of hearts  
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            if (i < stats.numberOfHearts)  //This is for creating our final health bar, change number of hearts to make amount visible in game 
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            
            gameManager.PlayerHasDied();
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Private Functions
    private void ChangeWeaponState(ElementType state)
    {  

        if(state == ElementType.Base)
        {
            print(state);
            element = ElementType.Water;
            color = Color.blue;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
        if(state == ElementType.Water)
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
        if(state == ElementType.Earth)
        {
            print(state);
            element = ElementType.Base;
            color = Color.black;
            material.color = color;
            Debug.Log("Switch to " + element);
        }
    }
    #endregion
}


public enum ElementType
{
    Base = 0,
    Water = 1,
    Fire = 2,
    Earth = 3,
   
}


