using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float distanceFromPlayer;
    public float talkRange;
    [Header("Animations")]
 

    [SerializeField] private Animator door;
    [SerializeField] private AnimationClip[] doorClips;

    public void Open()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        if (distanceFromPlayer <= talkRange)
            this.GetComponent<Animator>().Play(doorClips[0].name, 0, 0);
        else
            Debug.Log("Too Far Away");
    
    }
    private void OnTriggerExit(Collider other)
    {
        door.Play(doorClips[1].name, 0, 0);
        this.gameObject.GetComponent<BoxCollider>().enabled = false; // temporary fix for now if the game includes back tracking then we would need the doors to open and reopen 
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, player.position);

    }




}
