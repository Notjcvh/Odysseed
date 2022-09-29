using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("Refrences")]

    private PlayerController player;
    [SerializeField] private Transform orientation;
    private CameraController cam;
    [SerializeField] private Rigidbody rb;

    [Header("Ledge Detection")]
    [SerializeField] private float ledgeDetectionLegnth;
    [SerializeField] private float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    [Header("Ledge Grabbing")]
    [SerializeField] private float moveToLedgeSpeed;
    [SerializeField] private float maxLedgeGrabDistacne;

    public float minTimeOnLedge;
    private float timeOnLedge;
    
    public bool holding;
    

    //Variables
    private Transform lastLedge;
    private Transform currentLedge;
    private RaycastHit ledgeHit;

    [Header("Debugging")]
    public GameObject ledge;
    public GameObject wall;


    private void Start()
    {
        player = GetComponent<PlayerController>();
        cam = GetComponent<CameraController>();


    }

    private void Update()
    {

        LedgeDetection();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool anyKeyPressed =  verticalInput != 0;

        // Holding onto Ledge 
        if(holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            if (timeOnLedge > minTimeOnLedge && anyKeyPressed) ExitLedgeHold();
      
        }
    }



    private void LedgeDetection()
    {
       
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius,cam.CameraPlannerDirection, out ledgeHit, ledgeDetectionLegnth, whatIsLedge);
      
        if (!ledgeDetected) return;
        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);


        if (ledgeHit.transform == lastLedge) return;
        if(distanceToLedge < maxLedgeGrabDistacne && !holding) EnterLedgeHold();

    }


    private void EnterLedgeHold()
    {
        holding = true;
        

        currentLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        //Debugging
        Debug.Log("Entered Ledge Hold");
        Renderer renderer = ledge.GetComponent<Renderer>();
        renderer.material.color = Color.blue;
    }
    
    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;
        

        Vector3 directionToLedge = currentLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currentLedge.position);

        // Move player to Ledge
        if(distanceToLedge > 1f)
        {       
            if (rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 100f * Time.deltaTime);
                player.isRestricted = true;
                player.movementPriority = 1;
                cam.camPriority = 1;
        }

        if (distanceToLedge > maxLedgeGrabDistacne) ExitLedgeHold();

    }

    private void ExitLedgeHold()
    {
        holding = false;
        timeOnLedge = 0f;
        player.movementPriority = 0;
        cam.camPriority = 0;
        player.isRestricted = false;

        rb.useGravity = true;
    }

}
