using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Scenes/StartingPosition")]
public class Level: ScriptableObject
{

    public string sceneName;
    public string levelName;

    [TextArea]
    public string description;

}
