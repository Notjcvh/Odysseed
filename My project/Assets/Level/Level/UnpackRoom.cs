using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnpackRoom : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;
    public VectorValue level;
    public PuzzleDataManager whichPuzzle;

    public bool isRoomActive = false;
    public bool isRoomComplete = false;
    public List<GameObject> doors;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player","Ally","Enemy","SpecialEnemy","Door"};

    [Header("Room Type")]
    public bool isAPuzzleRoom;
    public bool isACombatRoom;
    

    [Header("Puzzle Variables")]
    public NPC puzzleDialouge;
    public int needMatchesToSolve;
    public int currentValue;
  

    [Header("Combat Variables")]
    public int lockNumber = 0;
    public bool allEnemiesDefeated = false;
    public List<GameObject> enemies = null;

    public GameEvent[] roomEvents;

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
                playerMovement.inCombatRoom = true;
                cam.camPriority = 1;
            }
        }
        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3])) // enemies
        {
            enemies.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().WhichRoom(this.gameObject.GetComponent<Collider>()); // sending the name of the trigger/(theroom) to the enemy
        }
        if (other.CompareTag(tags[4])) //doors
        {
            Add(other.gameObject.transform.parent.gameObject);

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
                playerMovement.inCombatRoom = false;
            }

            if (other.CompareTag(tags[2]) || other.CompareTag(tags[3]))
            {
                enemies.Remove(other.gameObject);
            }         
        }
    }

    public void TransportEnemy(GameObject other)
    {
      other.transform.position = level.boneYard;
    }

    public void GetTriggeredValue(int number)
    {
        currentValue += number;
    }

    public void Add(GameObject door)
    {
        if (!doors.Contains(door))
            doors.Add(door);
        if (doors.Count > 0)
        {
            doors.Sort(delegate (GameObject door1, GameObject door2)
            {
                DoorAnimation a;
                DoorAnimation b;

                a = door1.GetComponent(typeof(DoorAnimation)) as DoorAnimation;
                b = door2.GetComponent(typeof(DoorAnimation)) as DoorAnimation;

                return (a.doorValue).CompareTo(b.doorValue);
            });
        }
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
