using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Rooms/Type")]
public class RoomType : ScriptableObject
{
     
    public PuzzleDataManager whichPuzzle;

    //variables
    [Header("Tags")]
    public string[] tags = { "Player", "Enemy", "Ally" };



}
