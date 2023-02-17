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
    public PlayerMovement playerMovement;
    
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
            Debug.Log(weaponWheelSelected);

        }
        if (Input.GetButtonUp("Activate Seed Wheel"))
        {
            weaponWheelSelected = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log(weaponWheelSelected);
        }
            if (weaponWheelSelected == true)
        {
            anim.SetBool("OpenSeedWheel", true);
            SeedwheelIsActive();
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
                playerMovement.seedId = 1;
                break;
            case  2:
                noImage = selectedItem.sprite;
                playerMovement.seedId = 2;
                break;
            case 3:
                noImage = selectedItem.sprite;
                playerMovement.seedId = 3;
                break;
        }

        
    }


    #region Seed Wheel
    private void SeedwheelIsActive()
    {
        Camera cam = Camera.main;
        /*this.transform.LookAt(cam.transform);
        transform.RotateAround(player.transform.position, Vector3.up, 2 * Time.deltaTime);*/
        Vector3 wheel = this.transform.position;

        var ray = new Ray(wheel, cam.transform.position - this.transform.position);
        Debug.DrawRay(wheel, cam.transform.position - this.transform.position, Color.blue);

        Cursor.lockState = CursorLockMode.Confined;
        if (weaponWheelSelected == true)
        {
           /*

            Cursor.lockState = CursorLockMode.None;
           
            RaycastHit hit;

            // FOr later might have to corutine this or something else 
          
            
            if (Physics.Raycast(ray, out hit, 50f, playerObstructsUi, QueryTriggerInteraction.Ignore))
            {
                wheel = Vector3.MoveTowards(wheel, inFrontOfPlayer.position, .1f);
                seedWheel.transform.position = wheel;
                Debug.Log("yes");
            }
            else
            {
                wheel = Vector3.MoveTowards(wheel, behindPlayer.position, .1f);
                seedWheel.transform.position = wheel;
                Debug.Log("no");
            }*/

        }
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
