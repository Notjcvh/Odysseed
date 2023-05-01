using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThrone : MonoBehaviour
{
    public GameObject carrotKhan;
    public Transform wayPoint;
    void Update()
    {
        if(carrotKhan == null)
        {
            this.transform.position = wayPoint.transform.position;
        }
    }
}
