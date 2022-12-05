using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameManager gameManager;
   // private static Checkpoints instance;
    public bool reached = false;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        if(reached == true)
        {
            Destroy(this.gameObject);
            Debug.Log("this Checkpoint was destroyed" + this.gameObject); 
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        if(other.CompareTag("Player"))
        {
            //gameManager.lastCheckPointPos = transform.position;
            gameManager.hasSet.Add(this.transform);
            gameManager.Convert();
            reached = true;
        }

    }
}
