using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnablePlayParticles : MonoBehaviour
{
    public GameObject ps;
    private void OnEnable()
    {
        GameObject effectIns = Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(effectIns, 2f);
    }
}
