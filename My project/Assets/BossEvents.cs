using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvents : MonoBehaviour
{
    public SceneEvent[] sceneEvents;
    public GameEvent[] gameEvents;

    public void Call()
    {
        // Use the null conditional operator to check if the array is null
        for (int i = 0; i < sceneEvents?.Length; i++)
        {
            sceneEvents[i].Raise();
        }
        for (int i = 0; i < gameEvents?.Length; i++)
        {
            gameEvents[i].Raise();
        }
    }





}
