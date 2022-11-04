using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    public string[] tags = { "Player", "Enemy" };
    public GameObject[] enemies = null;
    
    [SerializeField] GameObject player;
    [SerializeField] CameraController cam;

    // Use this reffrence for dashing
    [SerializeField] PlayerMovement playerMovement;


    public Animator door = null;
    public GameObject doorAniamtionTrigger;

    public int lockNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        door = doorAniamtionTrigger.GetComponent<Animator>();

    }

    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag(tags[1]);
        lockNumber = enemies.Length;
        UnlockDoor();
    }

    private void UnlockDoor()
    {
        if (Input.GetKey(KeyCode.T))
        {


            door.Play("Door Open", 0, 0);
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
        }

    }

}
