using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{

    [Header("Animations")]
    [SerializeField] private Animator door;
    [SerializeField] private AnimationClip[] doorClips;
 


    public void Open()
    {
        door.Play(doorClips[0].name, 0, 0);
        Debug.Log("Hello");
    }
    private void OnTriggerExit(Collider other)
    {
        door.Play(doorClips[1].name, 0, 0);
        this.gameObject.GetComponent<BoxCollider>().enabled = false; // temporary fix for now if the game includes back tracking then we would need the doors to open and reopen 
    }

    public bool IsActive( bool Active)
    {
        bool IsActive = Active;
        return IsActive;
    }


}
