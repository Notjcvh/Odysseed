using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpackRoom : MonoBehaviour
{
    public RoomType room;
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;

    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private GameObject whichDoor;
    [SerializeField] private AnimationClip[] doorClips;

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
        door = whichDoor.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isACombatRoom == true)
        {
            lockNumber = enemies.Count;
        }
        else if (isAPuzzleRoom == true)
        {
            
            needMatchesToSolve = room.whichPuzzle.keywords.Length;
        }
        else
            return;
    }

    public void GetTriggeredValue(int number)
    {
        print(number);
        currentValue += number;
    }

    private void UnlockDoor()
    {
        if(isAPuzzleRoom && needMatchesToSolve == currentValue)
        {
            
            puzzleDialouge.puzzleCompleted = true;
            door.Play(doorClips[0].name, 0, 0);
        }

        if(isACombatRoom && lockNumber == 0)
        {
            door.Play(doorClips[0].name, 0, 0);
            allEnemiesDefeated = true;
        }

    }
    private void CloseDoor()
    {
        door.Play(doorClips[1].name, 0, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isAPuzzleRoom)
            cam.camPriority = 0;
            playerMovement.inCombatRoom = false;

        if (isACombatRoom)
        {
            playerMovement.inCombatRoom = true;
            cam.camPriority = 1;
            if (other.CompareTag(room.tags[1]))
            {
                enemies.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(isACombatRoom)
        {
            if (other.CompareTag(room.tags[0]))
            {
                cam.camPriority = 0;
                playerMovement.inCombatRoom = false;
            }


            if (other.CompareTag(room.tags[1]))
            {
                lockNumber -= 1;
                enemies.Remove(other.gameObject);
            }
        }
    }

    public void WhenTriggerEnter()
    {
        UnlockDoor();
    }
    public void WhenTriggerExit()
    {
        CloseDoor();
        
    }



}
