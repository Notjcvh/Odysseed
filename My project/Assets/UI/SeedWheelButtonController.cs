using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//For Later
public class SeedWheelButtonController : MonoBehaviour
{
    public int Id;
    private Animator anim;
    public string itemName; 
    public TextMeshProUGUI itemText;
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
        WeaponWheelController.weaponID = Id;
    }

    public void Deselected()
    {
        selected = false;
        WeaponWheelController.weaponID = 0;
    }


    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        Debug.Log("helllo");
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        Debug.Log("helllo");
        itemText.text = ""; // text empty 
    }

}
