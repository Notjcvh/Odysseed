using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnpackRoom : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;
  //  public VectorValue level;
    public PuzzleDataManager whichPuzzle;

    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player","Ally","Enemy","SpecialEnemy","Door"};

    [Header("Room Type")]
    public bool isAPuzzleRoom;
    public bool isACombatRoom;
    public GameEvent[] roomEvents; // what the doors are listening for


    [Header("Puzzle Variables")]
    public NPC puzzleDialouge;
    public int needMatchesToSolve; // this is what the room will check to open 
    public int currentValue;
  

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
        if (isACombatRoom == true)
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
        else if (isAPuzzleRoom == true)
        {
            needMatchesToSolve = whichPuzzle.keywords.Length;
            if (isRoomActive && needMatchesToSolve == currentValue)
            {
                isRoomComplete = true;
                for (int i = 0; i < roomEvents.Length; i++)
                {
                    roomEvents[i].Raise();
                    Debug.Log("Which Event" + roomEvents[i]);
                }
            }
                
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(tags[0])) // the player
        {
            isRoomActive = true;
          currentCoroutine = StartCoroutine(WaitFor());

            if (isAPuzzleRoom)
            {
                cam.camPriority = 0;
              
            }
            else if (isACombatRoom)
            {
                
                cam.camPriority = 1;
            }
        }
        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies
        {
            enemies.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().WhichRoom(this.gameObject); // sending to the enemy which room it is in
        }
        if (other.CompareTag(tags[4])) //doors
        {
           

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tags[0]))
        {
            isRoomActive = false;
            StopCoroutine(currentCoroutine);
        }

        if(isACombatRoom)
        {
            if (other.CompareTag(tags[0]))
            {
                cam.camPriority = 0;
               
            }
            if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies 
            {
                enemies.Remove(other.gameObject);
            }         
        }
    }

    public void TransportEnemy(GameObject other) // Transport the enemy to the boneyard 
    {
      //this function will activate the OnTriggerExit and Do:
      // remove the enemy from the list 
      // subtract 1 from the lock number 
  //    other.transform.position = level.boneYard; 
    }

    public void GetTriggeredValue(int number)
    {
        currentValue += number;
    }

    IEnumerator WaitFor() //checking if the room is complete
    { 
        while(isRoomActive && !isRoomComplete)
        {
            yield return new WaitForSeconds(timer);
            IsTheRoomComplete();
        } 
    }
}
