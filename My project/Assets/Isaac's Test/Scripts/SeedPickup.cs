using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    [Header("Ability Handler")]
    private AbilityHandler abilityHandler;
    public int abilityIndex;
    public SphereCollider _collider;
    public Rigidbody _rigidbody;
    public LayerMask ground;
    public RaycastHit hit;
    public float maxDistance;

    public bool triggered =false;


    //Tutorial
    [Header("Tutorail PopUp")]
    public TutorialPopUp tutorialPopUp;




    public float speed = 0f;
    
    public bool ForwardX = false;
    public bool ForwardY = false;
    public bool ForwardZ = false;

    public bool ReverseX = false;
    public bool ReverseY = false;
    public bool ReverseZ = false;

    // Start is called before the first frame update
    void Start()
    {
        abilityHandler = GameObject.FindGameObjectWithTag("AbilityHandler").GetComponent<AbilityHandler>();
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        if (tutorialPopUp != null)
            tutorialPopUp = GetComponentInChildren<TutorialPopUp>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggered = true;
            abilityHandler.AddAbility(abilityIndex);
            if (tutorialPopUp != null )
            {
                tutorialPopUp?.Activated();
            }
            else
                Destroy(this.gameObject);
        }
    }

    private void isGrounded()
    {
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(_collider.bounds.center, Vector3.down);

        if (Physics.Raycast(ray, out hit, maxDistance, ground, QueryTriggerInteraction.Ignore))
        {
            Vector3 hitPos = hit.point;
            float distance = Vector3.Distance(this.transform.position, hitPos);
        }
    }

    void Update()
    {
        //If triggerd is true and obj has a tutorial attached 
        if (triggered == true && tutorialPopUp.activated == false)
        {
            Debug.Log("Closed Tutorial");
            Destroy(this.gameObject);
        }


        isGrounded();
        float distance = Vector3.Distance(this.transform.position, hit.point);

        if (distance < 3)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;
            //   Debug.Log("true");
            speed = 35;
        }
        else
        {
            _rigidbody.useGravity = true;
            // modify the speed based on the distance to the ground
            speed = Mathf.Lerp(35f, 300f, Mathf.InverseLerp(0f, 3f, distance));
            speed = Mathf.Clamp(speed, 35f, 300f);
        }

        float s = Time.deltaTime * speed;

        if (ForwardX) transform.Rotate(s, 0, 0, Space.Self);
        if (ForwardY) transform.Rotate(0, s, 0, Space.Self);
        if (ForwardZ) transform.Rotate(0, 0, s, Space.Self);

        if (ReverseX) transform.Rotate(-s, 0, 0, Space.Self);
        if (ReverseY) transform.Rotate(0, -s, 0, Space.Self);
        if (ReverseZ) transform.Rotate(0, 0, -s, Space.Self);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 dir = Vector3.down * maxDistance;

        Gizmos.DrawRay(this.transform.position, dir);
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, ground, QueryTriggerInteraction.Ignore))
        {
            Vector3 hitPos = hit.point;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPos, 0.1f);

            float distance = Vector3.Distance(this.transform.position, hitPos);
            //    Debug.Log(distance);
            //modify the speed based on the distacne to the ground 
            if (distance < 3)
            {
                _rigidbody.useGravity = false;
            }
        }
    }


}
