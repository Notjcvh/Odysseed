using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    private GameObject player;
    private CameraController cam;
    private PlayerMovement playerMovement;

    public GameEvent[] roomEvents; // what the doors are listening for
    public VectorValue level;

    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player", "Ally", "Enemy", "SpecialEnemy", "Door" };

    [Header("Combat Variables")]
    public int lockNumber = 0;
    public List<GameObject> enemies = null; // this is what the room will check to open 

    private float timer = 1;
    Coroutine currentCoroutine = null;

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
    
            for (int i = 0; i < roomEvents.Length; i++)
            {
                roomEvents[i].Raise();
            }
        }
        else
            Debug.Log("Room is not complete");
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
            StopCoroutine(currentCoroutine);
            cam.camPriority = 0;
        }
        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies 
        {
            enemies.Remove(other.gameObject);
        }
    }

    public void TransportEnemy(GameObject other) // Transport the enemy to the boneyard 
    {
        //this function will activate the OnTriggerExit and Do:
        // remove the enemy from the list 
        // subtract 1 from the lock number 
        other.transform.position = level.boneYard;
    }
}

    
