using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom : MonoBehaviour
{
    public GameObject player;
    public CameraController cam;
    public PlayerMovement playerMovement;
    public VectorValue level;
    public PuzzleDataManager whichPuzzle;

    public GameEvent[] roomEvents; // what the doors are listening for

    public bool isRoomActive = false;
    public bool isRoomComplete = false;

    //variables
    [Header("Tags")]
    private string[] tags = { "Player", "Ally", "Enemy", "SpecialEnemy", "Door" };

    [Header("Puzzle Variables")]
    public NPC puzzleDialouge;
    public int needMatchesToSolve; // this is what the room will check to open 
    public int currentValue;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tags[0])) // the player
        {
            isRoomActive = true;
            currentCoroutine = StartCoroutine(WaitFor());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tags[0]))
        {
            isRoomActive = false;
            StopCoroutine(currentCoroutine);
        }
    }
    public void GetTriggeredValue(int number)
    {
        currentValue += number;
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
