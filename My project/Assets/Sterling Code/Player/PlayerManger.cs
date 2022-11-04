using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    //Handling Functions related to the player States 
    [Header("Refrences")]
    public PlayerStats stats;
    public ElementType element;

    //Heart Sprites
    [Header("Hearts Images")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart; 

    private void Update()
    {
       
        if(Input.GetKeyDown(KeyCode.E))
        {
            WeaponStateForward(element);
        }

        // Making sure our health equals the number of hearts 
        if(stats.health > stats.numberOfHearts)
            stats.health = stats.numberOfHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
            //Handling visually representing players health in realtion to number of hearts  
            if (i < stats.health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            //This is for creating our final health bar, change number of hearts to make amount visible in game 
            if (i < stats.numberOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    void WeaponStateForward(ElementType state)
    {
       
        print(state);
        if(state == ElementType.Base)
        {
            Debug.Log("Switch");
            element = ElementType.Water;
        }
        if(state == ElementType.Water)
        {
            Debug.Log("Switch");
            element = ElementType.Fire;
        }
        if (state == ElementType.Fire)
        {
            Debug.Log("Switch");
            element = ElementType.Earth;
        }
        if(state == ElementType.Earth)
        {
            Debug.Log("Switch");
            element = ElementType.Base;
        }

    }

}


public enum ElementType
{
    Base = 0,
    Water = 1,
    Fire = 2,
    Earth = 3,
    Air = 4
}


