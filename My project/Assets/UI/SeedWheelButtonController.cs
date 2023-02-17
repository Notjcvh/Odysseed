using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//For Later
public class SeedWheelButtonController : MonoBehaviour
{
    public int Id;
    public int prevId;
    private Animator anim;
    public string itemName; 
    public TextMeshProUGUI itemText;
    public WeaponWheelController weaponWheelController;
    public Image selecteditem;
    private bool selected = false;
    public Sprite icon; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            selecteditem.sprite = icon;
            itemText.text = itemName;
        }   
    }


    //Selecting and Deselecting our button 
    public void Selected()
    {
        selected = true;
        prevId = weaponWheelController.weaponID;
        weaponWheelController.weaponID = Id;
    }

    public void Deselected()
    {
        selected = false;
        weaponWheelController.weaponID = prevId;
    }


    public void HoverEnter()
    {
        /*
        anim.SetBool("Hover", true);
        itemText.text = itemName;
        */
        selected = true;
        prevId = weaponWheelController.weaponID;
        weaponWheelController.weaponID = Id;
    }

    public void HoverExit()
    {
        /*
        anim.SetBool("Hover", false);
        itemText.text = ""; // text empty 
        */
        selected = false;
        weaponWheelController.weaponID = prevId;
    }

}
