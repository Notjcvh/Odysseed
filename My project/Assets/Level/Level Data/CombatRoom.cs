using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    //Referencing 
    private GameObject player;
    private CameraController cam;
    private PlayerMovement playerMovement;

    // Room Checks
    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player", "Ally", "Enemy", "SpecialEnemy"};

    [Header("Combat Variables")]
    public int lockNumber = 0;
    public List<GameObject> enemies = null; // this is what the room will check to open 


    [Header("My Doors")]
    public DoorHandler[] myDoors;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(isRoomActive == true && isRoomComplete != true)
        {
            IsTheRoomComplete();
        }
    }

    private void IsTheRoomComplete()
    {
        lockNumber = enemies.Count;
        if (isRoomActive == true && lockNumber == 0)
        {
            isRoomComplete = true;
            for (int i = 0; i < myDoors.Length; i++)
            {
                myDoors[i].UnlockDoors();
                Debug.Log(myDoors[i]);
            }
        }
        else
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tags[0])) // the player
        {
            isRoomActive = true;
            cam.camPriority = 1;
        }
        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies
        {
            enemies.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().WhichRoom(this.gameObject); // sending to the enemy which room it is in
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tags[0]))
        {
            isRoomActive = false;
            cam.camPriority = 0;
        }
    }
}

    
