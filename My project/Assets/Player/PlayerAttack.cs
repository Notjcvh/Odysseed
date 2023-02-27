using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour 
{
    //Handle Inputs, Button Clicks, Collisions, and Possible Physics Applying Knockback 


    [Header("Referencing")]
    public Collider attackArea;
    public Transform playerPos;
    public Transform attackPosition;
    public Transform enmeyPosition;
    public LayerMask whatIsHittable;

    public Animator animator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput; 
    private Rigidbody obj;
    private Vector3 direction;

    [Header("Attack")]
    private int attackType;
    public float delayAttack = 1f;
    public int damage = 10;
  

    public Transform lerpPosition;
    public float lerpduration;
    public LayerMask playerCollionMask;
    IEnumerator co;

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;


    [Header("In Combo")]   
    public States previousState; //currentState
    public bool isAnimationActive;
    public bool isInComboState;

    public int fullcomboValue; 
    public int numberAttacksleft;


    public int nextAttack;
    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();  
    }

    void Update()
    {
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

        if (playerInput.attack)
        {
            Attack(1);
            Debug.Log("light attack");
            attackType = 1;
        }
        else if (playerInput.secondaryAttack)
        {
            Attack(2);
            Debug.Log("Heavy attack");
            attackType = 2;
        }


    }
    #endregion

    #region Attacks and Combos
    private void Attack(int attackType)
    {
        playerMovement.stopMovementEvent = true;
        // attackArea.enabled = true;
        //checking for attack Type and is Grounded for Knockback
        if (isInComboState == false) // light or heavy attack
        {
            //choose stater throw in own function
            switch (attackType, playerMovement.IsGrounded())
            {
                //ground attack checks 
                case (1, true):
                    animator.SetInteger("Attack Type", (1 << nextAttack));
                    previousState = States.LightStarter;
                    animator.SetBool("Active", true);
                    isInComboState = true;
                    numberAttacksleft += 1;
                    animator.SetBool("InCombo", isInComboState);
                    //Start timer
                    break;
                case (2, true):
                    //Debug.Log("Heavy Attadck Starter");
                    animator.SetInteger("Attack Type", (8 << nextAttack));
                    previousState = States.HeavyStarter;
                    animator.SetBool("Active", true);
                    break;
                // air attack checks
                case (1, false):
                    Debug.Log("Light Attack in Air Starter");
                    break;
                case (2, false):
                    Debug.Log("Heavy Attack in Air Starter");
                    break;
                default:
                    Debug.Log("null");
                    break;
            }
        }
        else if (isInComboState == true)
        {
            nextAttack = NextAttack(previousState);
            animator.SetInteger("Attack Type", nextAttack);
            animator.SetBool("Active", true);           
        }
        OnTriggerEnter(attackArea);
        StartCoroutine(DelayAttack());
    }

    // lights attacks
    //heavy attacks


    private bool IsComboComplete(int fullComboString, int num)
    {
        if (num > fullComboString)
        {
            return true;
        }
        else
            return false;
    }

    //delete
    private int NextAttack(States currentState)
    {

        List<short> copy = GetStates(currentState).ToList();
        List<short> statesCalled = new List<short>();

        int previousStatesCalled = Convert.ToInt32(currentState);
        int count = previousStatesCalled;

        int whatAttackType = attackType;

        statesCalled.Add(((short)previousStatesCalled));

        fullcomboValue = copy.LastOrDefault();

        foreach (var item in copy)
        {
            if ((item << 1 & (int)previousStatesCalled) == previousStatesCalled - previousStatesCalled) // finding next attack 
            {
                int newAttackState = item << 1;
                //Debug.Log(Convert.ToString(newAttackState, 2).PadLeft(32, '0'));\
               
                previousState = ((States)newAttackState);
                return newAttackState;
            }                
        }
        return 0;
    }
            
    static IEnumerable<Int16> GetStates(States previousState)
    {
        
        foreach (States value in Enum.GetValues(previousState.GetType()))
            if ((value & previousState) == previousState) //if the bits are the same  we are going to add to list
            {
                yield return (short)value;
            }
    }
   

    public void isAnimationFinished()
    {
        playerMovement.stopMovementEvent = false;
        animator.SetBool("Active", false);
        isAnimationActive = false;
    }

    public void FinishedCombo()
    {
        animator.SetBool("InCombo", false);
        nextAttack = 0;
        animator.SetInteger("Attack Type", nextAttack);
        isInComboState = false;
       previousState = States.None;
    }

    // how does this affect attacking 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
    }

    #region Sliding Forward When Attacking 
    public void SlideForward()
    {
        co = MoveForwardWhenAttacking(transform.position, lerpPosition.position, lerpduration);
        StartCoroutine(co);
    }
    private IEnumerator MoveForwardWhenAttacking(Vector3 currentPostion, Vector3 endPosition, float time)
    {
        RaycastHit hit;
        float range = 2f;
        Ray ray = new Ray(currentPostion, transform.TransformDirection(Vector3.forward * range));

        Vector3 midpoint = Vector3.Lerp(currentPostion, endPosition, .5f);
        //this is for sliding 
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            if (Physics.Raycast(ray, range, playerCollionMask, QueryTriggerInteraction.Ignore))
            {
                transform.position = currentPostion;
            }
            else
                transform.position = Vector3.Lerp(currentPostion, midpoint, t);
            yield return null;
        }
    }
    #endregion


    #region Move somewhere else 
    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if (whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
        {
            obj = attackArea.gameObject.GetComponent<Rigidbody>();
            if (obj != null)
                HitSomething(direction, obj);
            else
                return;
        }
    }

    // Create Stun Stop enemey
    private void HitSomething(Vector3 direction, Rigidbody obj)
    {

        //int attackTypeConnected = attackType; // might be a problem with multiple clicks 


        //if tag is enemy
        if (obj.tag == "Enemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<Enemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);

        }
        else if (obj.tag == "SpecialEnemy")
        {
            DamagePopUp.Create(obj.transform.position, damage);
            obj.SendMessage("DisableAI");
            obj.gameObject.GetComponent<SpecialEnemy>().ModifiyHealth(damage / 10);
            obj.gameObject.GetComponent<EnemyStats>().VisualizeDamage(obj);
            obj.SendMessage("TakeDamage", damage / 10);
        }
        if (obj.tag == "Boss")
        {
            obj.SendMessage("TakeDamage", damage / 10);
        }
    }

    private void AddKnockback()
    {
        direction.y = 0;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    private void AddKnockUp()
    {
        direction.x = 0;
        direction.z = 0;
        direction.y = 1;
        obj.AddForce(direction * knockbackStrength, ForceMode.Impulse);
    }
    #endregion
    #endregion
}


[System.Flags]
public enum States
{
    //Powers of two
    None = 0,
    //Individual Attacks
    LightStarter = 1 << 0, // 1
    LightCombo1 = 1 << 1,  //2
    LightFinisher = 1 << 2, //4
    
    HeavyStarter = 1 << 3, //8
    HeavyFinished = 1  << 4, //16

    


    //Full Combo Strings  delete

    fullLightCombo = LightStarter | LightCombo1 | LightFinisher,
   // LightHeavy = LightStarter | LightCombo1 | HeavyStarter,
    fullHeavyCombo = HeavyStarter | HeavyFinished
}
