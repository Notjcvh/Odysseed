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

    // second collider to launch heavy 
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
  //  public Transform orgLerpPos;
    public float lerpduration;
    public LayerMask playerCollionMask;
    IEnumerator co;

    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    [Header("Knockback")]
    public float knockbackTimer;
    public float knockbackStrength;

    public States states;
    public States startingState;
  
    public bool isInComboState;
    private int fullComboValue;

    public int numberofAttacksLeft;



    #region Unity Functions
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerInput = GetComponent<PlayerInput>();

        attackArea.enabled = false;      
    }


    // Update is called once per frame
    void Update()
    {
        if(obj != null)
          direction = (obj.transform.position - attackPosition.position).normalized; // finding the direction from attackPos to Obj rigidbody. In update so Knockback happen for the full time between frames

        if (playerInput.attack)
        {
            Attack(0, states);
        }
        else if (playerInput.secondaryAttack)
        {
            Attack(1, states);
        }
    }
    #endregion

    #region Private Functions
    private void Attack(int attackType, States currentState)
    {
        playerMovement.stopMovementEvent = true;
       // attackArea.enabled = true;
      
         //checking for attack Type and is Grounded for Knockback
        bool isAnimationActive = animator.GetBool("Active");

        if(isInComboState == false)
        {
            //choose possible combo
            switch (attackType, playerMovement.IsGrounded())
            {
                //ground attack checks 
                case (0,  true):
                    states = States.fullLightCombo;
                    animator.SetInteger("Attack Type", ComboStringBreakdown(states, States.LightStarter));
                    animator.SetBool("Active", true);
                    startingState = States.LightStarter;
                     isInComboState = true;
                    //Start timer
                    break;
                case (1,  true):
                    //Debug.Log("Heavy Attadck Starter");
                    states = States.fullHeavyCombo;
                    animator.SetInteger("Attack Type", ComboStringBreakdown(states, States.HeavyStarter));
                    animator.SetBool("Active", true);
                    startingState = States.HeavyStarter;
                    isInComboState = true;
                    break;
                // air attack checks
                case (0,  false):
                    Debug.Log("Light Attack in Air Starter");
                    break;
                case (1, false):
                    Debug.Log("Heavy Attack in Air Starter");
                    break;
                default:
                    Debug.Log("null");
                    break;
            }

        }
        else
        {
            if(numberofAttacksLeft != 0)
            {
                numberofAttacksLeft -= 1;
                int bitSet = 1 << 1;
                Debug.Log( Convert.ToString(bitSet, 2).PadLeft(32, '0'));
                // bit shift 
            }

            // compare


            //then hash? 
        }
        OnTriggerEnter(attackArea);
        StartCoroutine(DelayAttack());
    }

                                    //full combo
    int ComboStringBreakdown(States currentstate, States starterAttack)
    {
        int comboValue = ((int)currentstate);
        int startervalue = ((int)starterAttack);
        fullComboValue = comboValue;

        string L = Convert.ToString(fullComboValue, 2).PadLeft(32, '0');
        string H = Convert.ToString(startervalue, 2).PadLeft(32, '0');

        //count all the ones then add 
        int count = 0;
    
        foreach (var item in L)
        {
            if(item == '1')
              count++;
        }

        numberofAttacksLeft = count -1;
       // print(fullComboValue + " this value " + startervalue);
       // print(L);
        print(H);
        return startervalue;
    }




    public void isAnimationFinished()
    {
        playerMovement.stopMovementEvent = false;
        attackArea.enabled = false;
        animator.SetBool("Active", false);

    }

    // how does this affect attacking 
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayAttack);
    }

    #region Move somewhere else 
    private void OnTriggerEnter(Collider attackArea) // if an object has collided with the attacksphere while it is active 
    {
        // if the 3rd hit 
        if(whatIsHittable == (whatIsHittable | (1 << attackArea.transform.gameObject.layer))) // Bitwise equation: layermask == (layermask | 1 << layermask)
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
    #endregion
    #endregion

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
}


[System.Flags]
public enum States
{
    None = 0,
    LightStarter = 1 << 0,
    LightCombo1 = 1 << 1,
    LightFinisher = 1 << 2,
    //3
    HeavyStarter = 1 << 3,
    HeavyFinished = 1  << 4,

    //2

    fullLightCombo = LightStarter | LightCombo1 | LightFinisher,
    fullHeavyCombo = HeavyStarter | HeavyFinished
}
