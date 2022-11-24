using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpackRoom : MonoBehaviour
{
    public RoomType room;


    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private GameObject whichDoor;
    [SerializeField] private AnimationClip[] doorClips;


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
        room.cam = room.player.GetComponent<CameraController>();
        door = whichDoor.GetComponent<Animator>();
        if (room.isACombatRoom == true)
        {
            lockNumber = enemies.Count;



        }
        else if (room.isAPuzzleRoom == true)
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
        if(room.isAPuzzleRoom && needMatchesToSolve == currentValue)
        {
            
            puzzleDialouge.puzzleCompleted = true;
            door.Play(doorClips[0].name, 0, 0);
        }



        if(room.isACombatRoom && lockNumber == 0)
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
        if (room.isAPuzzleRoom)
            room.cam.camPriority = 0;

        if(room.isACombatRoom)
        {
            if (other.CompareTag(room.tags[0]))
            {
                if (room.cam != null)
                {
                    room.cam.camPriority = 1;
                }

            }
            if (other.CompareTag(room.tags[1]))
            {
                enemies.Add(other.gameObject);
            }
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if(room.isACombatRoom)
        {
            if (other.CompareTag(room.tags[0]))
            {
                room.cam.camPriority = 0;
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
