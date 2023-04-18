using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameManager gameManager;
    public Collider Trigger;
    public bool reached = false;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void Update()
    {
        if (reached == true)
        {
            this.gameObject.SetActive(false);

        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gameManager.lastCheckPointPos = transform.position;
            gameManager.hasSet.Add(this.transform.position);
            gameManager.Convert();
            reached = true;
        }
    }

}

