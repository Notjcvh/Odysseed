using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
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


        switch (weaponID)
        {
             case 0: //NOTHING IS SELECTED
                selectedItem.sprite = noImage;
                break;
            case 1:
                Debug.Log("Water Seed"); // here is where we can call animations 
                selectedItem.sprite = noImage;
                break;
            case  2:
                Debug.Log("Earth Seed");
                selectedItem.sprite = noImage;
                break;
            case 3:
                Debug.Log("Sun Seed");
                selectedItem.sprite = noImage;
                break;
        }

        
    }
}
