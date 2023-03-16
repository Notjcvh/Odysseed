using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Scenes/StartingPosition")]
public class Level: ScriptableObject
{

    public string sceneName;
    public string levelName;
    public Vector3 initialStartValue;

 


    [TextArea]
    public string description;

}
