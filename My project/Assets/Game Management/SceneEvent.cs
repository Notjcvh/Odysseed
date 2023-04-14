using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneEvent", menuName = "Events/SceneEvent")]
public class SceneEvent : ScriptableObject
{
    private readonly List<GameEventListener> eventListeners = new List<GameEventListener>(); //can only be accesed not modified
    
    


    // start the broadcast play the event
    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--) // this for loop runs in reverse 
           eventListeners[i].OnSceneEventRaised(this);
    }

    // add to list on enabled, play event when raised, then remove listener from the list 
    public void RegisterListener(GameEventListener listener)
    {

        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }


}
