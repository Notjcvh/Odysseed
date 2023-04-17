using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneData
{
    None,
    #region BuildScenes
    Title,
    Vineyard,
    Dungeon1,
    Vineyard2,
    PotatoLands,
    Dungeon2,
    CarrotKhanate,
    Dungeon3,
    #endregion
    #region Testing Scenes 
    Sterling_PlayerTestScene,

    #endregion 
}

[CreateAssetMenu(fileName = "Level",menuName = "Scenes/LevelData")]
public class Level: ScriptableObject
{

    public SceneData sceneName;
    public string levelName;

    [TextArea]
    public string description;

}
