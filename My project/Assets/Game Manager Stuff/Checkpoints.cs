using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameManager gameManager;
    private static Checkpoints instance;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            //gameManager.lastCheckPointPos = transform.position;
            gameManager.hasSet.Add(this.transform);
            gameManager.Convert();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }
}
