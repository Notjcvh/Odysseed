using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Rooms/Type")]
public class RoomType : ScriptableObject
{
    // members
    [Header("Refrencing")]

    [SerializeField] public GameObject player;
    [SerializeField] public CameraController cam;
    [SerializeField] public PlayerMovement playerMovement;
    
    public PuzzleDataManager whichPuzzle;

    //variables
    [Header("Tags")]
    public string[] tags = { "Player", "Enemy", "Ally" };

    [Header("Room Type")]
    public bool isAPuzzleRoom;
    public bool isACombatRoom;


}
