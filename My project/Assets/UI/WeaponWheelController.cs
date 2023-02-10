using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    public static bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID; // For later

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }


        if (weaponWheelSelected)
        {
            anim.SetBool("OpenSeedWheel", true);
        }
        else
        {
            anim.SetBool("OpenSeedWheel", false);
        }


        switch (weaponID) // For Later switch Statement 
        {
             case 0: //NOTHING IS SELECTED
                noImage = selectedItem.sprite;
                break;
            case 1:
               // Debug.Log("Water Seed"); // here is where we can call animations 
                noImage = selectedItem.sprite;
                break;
            case  2:

                noImage = selectedItem.sprite;
                break;
            case 3:
                noImage = selectedItem.sprite;
                break;
        }

        
    }
}
