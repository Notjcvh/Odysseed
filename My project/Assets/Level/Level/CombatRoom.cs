using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    private GameObject player;
    private CameraController cam;
    private PlayerMovement playerMovement;

    public DoorHandler[] doors; // what the doors are listening for
    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player", "Ally", "Enemy", "SpecialEnemy", "Door" };

    [Header("Combat Variables")]
    public int lockNumber = 0;
    public List<GameObject> enemies = null; // this is what the room will check to open 

  //private float timer = 1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        IsTheRoomComplete();
    }

    private void IsTheRoomComplete()
    {
        lockNumber = enemies.Count;
        if (isRoomActive == true && lockNumber == 0)
        {
            isRoomComplete = true;

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].UnlockDoors();
            }
        }
        else
            return;
            //Debug.Log("Room is not complete");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tags[0])) // the player
        {
            isRoomActive = true;


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
        }
        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies 
        {
            enemies.Remove(other.gameObject);
        }
    }

}

    
