using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnpackRoom : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;
    public VectorValue level;
    public GameEvent openDoor;
    public PuzzleDataManager whichPuzzle;
   

    public bool isRoomActive = false;
    public bool wasFunctionCalled = false;
    //variables
    [Header("Tags")]
    private string[] tags = { "Player","Ally","Enemy","SpecialEnemy" };

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


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        
    }

    private void Update()
    {
        if (isRoomActive)
        {
            if (isACombatRoom == true)
            {
                lockNumber = enemies.Count;
               if( isRoomActive && lockNumber == 0  && isACombatRoom && !wasFunctionCalled)
                {
                    IsTheRoomComplete();
                    wasFunctionCalled = true;
                }
            }
            else if (isAPuzzleRoom == true)
            {
                needMatchesToSolve = whichPuzzle.keywords.Length;
                if (isRoomActive && needMatchesToSolve == currentValue && isAPuzzleRoom &&!wasFunctionCalled)
                {
                    IsTheRoomComplete();
                    wasFunctionCalled = true;
                }
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                IsTheRoomComplete();
            }
        }
        else
            return;
    }

    public void GetTriggeredValue(int number)
    {
        print(number);
        currentValue += number;
    }

    private void IsTheRoomComplete()
    {
        openDoor.Raise();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(tags[0]))
        {
            isRoomActive = true;

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

        if (other.CompareTag(tags[2]) || other.CompareTag(tags[3]))
        {
            enemies.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().WhichRoom(this.gameObject.GetComponent<Collider>()); // sending the name of the trigger/(theroom) to the enemy
        }
    }


    private void OnTriggerExit(Collider other)
    {
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
                lockNumber -= 1;
            }         
        }
    }

    public void TransportEnemy(GameObject other)
    {
      other.transform.position = level.boneYard;
    }



}
