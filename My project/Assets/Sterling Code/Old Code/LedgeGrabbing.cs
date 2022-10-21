using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("Refrences")]

    public GameObject ledgeGrabPosition;
    private PlayerMovement player;
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
    public bool canDetect;
    

    [Header("Ledge Jumping")]
    KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float ledgeJumpVerticalVelocity = 10;

    [Header("Exiting")]
    public bool exitingLedge;
    public float exitLedgeTime;
    private float exitLedgeTimer;

    //Variables
    private Transform lastLedge;
    private Transform currentLedge;
    private RaycastHit ledgeHit;
    public float distance = 1;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        cam = GetComponent<CameraController>();
    }

    private void Update()
    {
        CheckingDetectLedge();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
       
        float verticalInput = Input.GetAxis("Vertical");
        bool anyKeyPressed =  verticalInput != 0;

        // Holding onto Ledge 
        if(holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            if (timeOnLedge > minTimeOnLedge && anyKeyPressed)
            {
                ExitLedgeHold();

            }
            if (Input.GetKeyDown(jumpKey)) LedgeJump();
        }
    }

    void CheckingDetectLedge()
    {

        if (exitingLedge == true)
        {
            StartCoroutine(Blackhole());
            exitingLedge = false;
        }
        else
            LedgeDetection();
        IEnumerator Blackhole()
        {
            yield return new WaitForSeconds(exitLedgeTime); // waits before continuing in seconds
                              
        }
    }
    private void LedgeDetection()
    {
       
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius,cam.CameraPlannerDirection, out ledgeHit, ledgeDetectionLegnth, whatIsLedge);
      
        if (!ledgeDetected) return;
        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if(distanceToLedge < maxLedgeGrabDistacne && !holding) EnterLedgeHold();
    }


    private void EnterLedgeHold()
    {
        

        currentLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;      

        holding = true;

    }

    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;

        Vector3 directionToLedge =  (currentLedge.position - transform.position);
        float distanceToLedge =  Vector3.Distance(transform.position, currentLedge.position);

        // Move player to Ledge
        if (distanceToLedge > 1f)
        {       
            if (rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 100f * Time.deltaTime);
                player.isRestricted = true;
                player.movementPriority = 1;
                cam.camPriority = 1;
        }

        if (distanceToLedge > maxLedgeGrabDistacne) ExitLedgeHold();

    }

    private void LedgeJump()
    {
        ExitLedgeHold();
        rb.AddForce(Vector3.up * ledgeJumpVerticalVelocity, ForceMode.Impulse);
    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        holding = false;
        timeOnLedge = 0f;     
        player.isRestricted = false;
        player.movementPriority = 0;
        cam.camPriority = 0;
        rb.useGravity = true;
    }

  

    private void ExitClimbState()
    {

        
    }

}
