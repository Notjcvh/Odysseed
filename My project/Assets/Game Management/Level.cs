using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level",menuName = "Scenes/LevelData")]
public class Level: ScriptableObject
{
    public SceneData sceneName;
    public GameObject levelArea;
}
