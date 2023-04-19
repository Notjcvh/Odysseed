using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameEvent clearCheckpoints;
    public SceneData sceneToLoad;
    public VectorValue intialPosition;
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

            //clear checkpoint list
            clearCheckpoints.Raise();


            string sceneName = sceneToLoad.ToString();
            gameManager.LoadLevel(sceneName);
        }
    }
}
