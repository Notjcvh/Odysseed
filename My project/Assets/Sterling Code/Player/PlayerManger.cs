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

    [Header("Hearts Images")]     //Heart Sprites
    public Image[] hearts; // the full array of hearts in the game
    public Sprite fullHeart;
    public Sprite emptyHeart;

    #region Unity Functions
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
    }
    #endregion

    #region Private Functions
    private void ChangeWeaponState(ElementType state)
    {  

        if(state == ElementType.Base)
        {
            print(state);
            element = ElementType.Water;
            Debug.Log("Switch to " + element);
        }
        if(state == ElementType.Water)
        {
            element = ElementType.Fire;
            Debug.Log("Switch to " + element);
        }
        if (state == ElementType.Fire)
        {
            element = ElementType.Earth;
            Debug.Log("Switch to " + element);
        }
        if(state == ElementType.Earth)
        {
            print(state);
            element = ElementType.Base;
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


