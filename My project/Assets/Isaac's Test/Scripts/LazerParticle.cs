using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerParticle : MonoBehaviour
{
    public Transform lookatTarget;
    void Update()
    {
        this.transform.LookAt(lookatTarget);
    }
}
