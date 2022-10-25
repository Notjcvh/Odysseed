using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    public string[] tags = { "Player", "Enemy" };
    [SerializeField] GameObject player;
    [SerializeField] CameraController cam;
    [SerializeField] PlayerMovement playerMovement;

     


    // Start is called before the first frame update
    void Start()
    {
        cam = player.GetComponent<CameraController>();
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

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
    }

}
