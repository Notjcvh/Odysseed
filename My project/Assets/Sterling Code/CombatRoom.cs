using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    public string[] tags = { "Player", "Enemy" };
    public Collider DoorCol;
    public MeshRenderer DoorMesh;
    public GameObject[] enemies = null;



    
    [SerializeField] GameObject player;
    [SerializeField] CameraController cam;
    [SerializeField] PlayerMovement playerMovement;


   public int lockNumber = 0;

     


    // Start is called before the first frame update
    void Start()
    {
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();


    }

    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag(tags[1]);
        lockNumber = enemies.Length;


        if(lockNumber == 0)
        {
            DoorCol.enabled = false;
            DoorMesh.enabled = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(tags[0]))
        {
            if(cam != null)
            {
                Debug.Log("Player has enetered the room, enter combat mode");

                cam.camPriority = 1;
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag(tags[0]))
        {
            Debug.Log("Player has exited the room, return to exploration mode");
            cam.camPriority = 0;
        }


        if (other.CompareTag(tags[1]))
        {
            lockNumber -= 1;
        }

    }

}
