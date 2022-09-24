using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
   /*[Header("Refrenceing")]

    private CameraController cam;
    [SerializeField] GameObject player;
    [SerializeField] GameObject sphere;



    bool isActive = false;
    
    Transform playerTrans;
    Vector3 worldPosition;
    public float planeDistanceZ;
    
    

    private void Start()
    {
        cam = player.gameObject.GetComponent<CameraController>();
        playerTrans = player.gameObject.transform;
        sphere.gameObject.SetActive(false);
       
    }

    private void Update()
    {
        if(cam.camPriority == 1)
        {
            
            isActive = true;
            if(isActive == true)
            {
                sphere.SetActive(true);
                
                
                MouseToWorld();
                InCombat();
            }
            else
            {
                sphere.SetActive(false);
            }
        }
    }

    private void InCombat()
    {
        // Yes it works but it needs more conditions
        playerTrans.LookAt(this.gameObject.transform, Vector3.up);

    }

    private void MouseToWorld()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + planeDistanceZ;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition.y = 1.95f;
        gameObject.transform.position = worldPosition;
        Debug.Log(worldPosition);
    }*/
    
}
