using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossHealth;
    public SceneEvent sceneEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            boss.SetActive(true);
            bossHealth.SetActive(true);
            sceneEvent?.Raise();
            this.gameObject.SetActive(false);
        }
    }
}
