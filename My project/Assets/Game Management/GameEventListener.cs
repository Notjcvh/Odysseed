
//------------------------------------------------------
//Came From 
//https://github.com/roboryantron/Unite2017
//
// Unite 2017 - Game Architecture with Scriptable Objects 
//
//Author: Ryan Hipple 
//Date:  10/04/17 
//------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvents[] myGameEvent;
    public PlayerEvents[] myPlayerEvent;   
   


    [System.Serializable]
    public class GameEvents
    {
        public GameEvent Event;
        public UnityEvent Response;
    }

    [System.Serializable]
    public class PlayerEvents
    {
        public PlayerEvent Event;
        public UnityEvent Response;
    }
    private void OnEnable()
    {
        foreach (GameEvents item in myGameEvent)
        {
            item.Event?.RegisterListener(this);
        }
        foreach (PlayerEvents item in myPlayerEvent)
        {
            item.Event?.RegisterListener(this);
        }
    }

    private void OnDisable()
    {

        foreach (GameEvents item in myGameEvent)
        {
            item.Event?.UnregisterListener(this);
        }
        foreach (PlayerEvents item in myPlayerEvent)
        {
            item.Event?.UnregisterListener(this);
        }
    }


    #region CallEvents
    public void OnGameEventRaised(GameEvent gameEvent)
    {
        foreach (GameEvents item in myGameEvent)
        {
            if(item.Event.name == gameEvent.name)
            {
                item.Response.Invoke();
                return;
            }
        }
    }
    public void OnPlayerEventRaised(PlayerEvent playerEvent)
    {
        foreach (PlayerEvents item in myPlayerEvent)
        {
            if(item.Event.name == playerEvent.name)
            {
                item.Response.Invoke();
                return;
            }
        }
    }
    #endregion
}
