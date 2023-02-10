using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;
    public VectorValue level;


    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player", "Ally", "Enemy", "SpecialEnemy", "Door" };

    public GameEvent[] roomEvents; // what the doors are listening for

    [Header("Combat Variables")]
    public int lockNumber = 0;
    public bool allEnemiesDefeated = false;
    public List<GameObject> enemies = null; // this is what the room will check to open 

    private float timer = 1;
    Coroutine currentCoroutine = null;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void IsTheRoomComplete()
    {
        lockNumber = enemies.Count;
        if (isRoomActive && enemies.Count == 0)
        {
            isRoomComplete = true;
            for (int i = 0; i < roomEvents.Length; i++)
            {
                roomEvents[i].Raise();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tags[0])) // the player
        {
            isRoomActive = true;
            currentCoroutine = StartCoroutine(WaitFor());
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
    IEnumerator WaitFor() //checking if the room is complete
    {
        while (isRoomActive && !isRoomComplete)
        {
            yield return new WaitForSeconds(timer);
            IsTheRoomComplete();
        }
    }
}
