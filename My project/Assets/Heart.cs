using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private PlayerManger playerManger;
    // Start is called before the first frame update
    void Start()
    {
        playerManger = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerManger.RestoreHealth();
        Destroy(this.gameObject);
    }
}
