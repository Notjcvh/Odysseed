using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
   
    public bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public int weaponID;

    public GameObject player;
    public PlayerInput playerInput;
    public CharacterStatus characterStatus;


    public Animator[] animators;
    
    [Header("Weapon Wheel Ui")]
    public LayerMask playerObstructsUi; // have it check for player layer 
    public Transform behindPlayer; // starting point 
    public Transform inFrontOfPlayer; // lerp point

    private void Start()
    {
        
    }

    void Update()
    {
       if(Input.GetButtonDown("Activate Seed Wheel"))
        {
            weaponWheelSelected = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetButtonUp("Activate Seed Wheel"))
        {
            weaponWheelSelected = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
            if (weaponWheelSelected == true)
        {
            anim.SetBool("OpenSeedWheel", true);
        }
        else
        {
            anim.SetBool("OpenSeedWheel", false);
            switch (weaponID) // For Later switch Statement 
            {
                case 0: //NOTHING IS SELECTED
                    noImage = selectedItem.sprite;
                    break;
                case 1:
                    // Debug.Log("Water Seed"); // here is where we can call animations 
                    noImage = selectedItem.sprite;
                    characterStatus.seedId = 1;
                    break;
                case 2:
                    noImage = selectedItem.sprite;
                    characterStatus.seedId = 2;
                    break;
                case 3:
                    noImage = selectedItem.sprite;
                    characterStatus.seedId = 3;
                    break;
            }
            characterStatus.SwitchSeed();
        }


        

        
    }
}
