using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    
    public List<GameObject> enemies = null;
    
    [SerializeField] GameObject player;
    [SerializeField] CameraController cam;
    public AnimationClip[] clips;


    // Use this reference for dashing
    [SerializeField] PlayerMovement playerMovement;


    public Animator door = null;
    public GameObject doorAniamtionTrigger;

    public string[] tags = { "Player", "Enemy" };
    public int lockNumber = 0;
    public bool allenemiesDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        door = doorAniamtionTrigger.GetComponent<Animator>();

    }

    private void Update()
    {
       
        lockNumber = enemies.Count;
    }

    private void UnlockDoor()
    {
        
        if(lockNumber == 0)
        {
            door.Play(clips[0].name, 0, 0);
            allenemiesDefeated = true;
        }
    }
    private void CloseDoor()
    {
       if(allenemiesDefeated == true)
        {
            door.Play(clips[1].name, 0, 0);
            allenemiesDefeated = false;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(tags[0]))
        {
            if(cam != null)
            {
                cam.camPriority = 1;
            }
           
        }
        if(other.CompareTag(tags[1]))
        {
            enemies.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag(tags[0]))
        {
            cam.camPriority = 0;
        }


        if (other.CompareTag(tags[1]))
        {
            lockNumber -= 1;
            enemies.Remove(other.gameObject);
        }

    }

    public void WhenTriggerEnter()
    {
        UnlockDoor();
    }
    public void WhenTriggerExit()
    {
        CloseDoor();
    }



}
