using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBoss : MonoBehaviour
{
    public GameObject boss;


    private void OnTriggerEnter(Collider other)
    {
        boss.SetActive(true);
    }
}
