using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AssignObjective : MonoBehaviour, IAssignable
{
    public PlayerEventsWithData playerEvent;
    [TextArea]
    public string objective;


    public void GiveObjective()
    {
        if (playerEvent != null)
        {
            //Assign the scriptable object event string with the our objective string 
            playerEvent.text = objective;
            AssignObjective(playerEvent);
        }
    }

    public void AssignObjective(PlayerEventsWithData playerEvent)
    {
        playerEvent.Raise();
    }
}
