using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemySummon : MonoBehaviour
{
    public NavMeshAgent nma;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        nma = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        nma.enabled = false;
    }

    public void StartAi()
    {
        nma.enabled = true;
        anim.enabled = false;
    }
}
