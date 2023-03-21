
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
    public List<GameEvent> Event; // Game Event to register with
    public List<PlayerEvents> playerEvents;

    public UnityEvent Response; // response to invoke when event is raised in the inspector 

    private void OnEnable()
    {
        foreach (var item in Event)
        {
            item?.RegisterListener(this);
        }

        foreach (var item in playerEvents)
        {
            item?.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (var item in Event)
        {
            item?.UnregisterListener(this);
        }

        foreach (var item in playerEvents)
        {
            item?.RegisterListener(this);
        }
    }

    public void OnEventRaised()
    {
        Response.Invoke(); // calls the code do something 
    }

}
