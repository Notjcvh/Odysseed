using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AdditionalActions: MonoBehaviour, IAssignable, ISpeakable

{
    public bool willAssignObjective;
    public bool willSpeak;
 
    [TextArea]
    public string objective;

    public void AssignObjective(PlayerEventsWithData assignObjective)
    {
        //Assign the scriptable object event string with the our objective string 
        assignObjective.text = objective;
        assignObjective.Raise();
    }

    public void Talk(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}


public interface ISpeakable
{
    void Talk(AudioSource audioSource, AudioClip audioClip);
    //void AssignObjective(PlayerEventsWithData assignObjective);
}
