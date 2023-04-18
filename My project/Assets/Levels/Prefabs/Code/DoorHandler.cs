using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject encounterCollider; // this object, for backtracking 

    [Header("Animations")]
    public Animator door;
    public AudioSource audioSource;
    public AudioClip doorSound;

    public bool locked = true;
    
    private Material material;

    private void Start()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        if (locked != true)
        {
            material.SetColor("_Color", Color.green);
        }

       // Get the AudioSource component attached to this GameObject
       audioSource = GetComponent<AudioSource>();

       // Assign the AudioClip to the AudioSource
       audioSource.clip =doorSound;
        
    }
    public virtual void UnlockDoors()
    {
        locked = false;
        material.SetColor("_Color", Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            Open();
        }
    }
    public virtual void Open()
    {
        door.SetBool("Open", true);
       
        door.SetBool("Close", false);
        audioSource.Play();
    }

    public virtual void Close()
    {
        door.SetBool("Close", true);
        door.SetBool("Open", false);
        audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && locked == false)
        {
            Close();
        }   
    }
}
