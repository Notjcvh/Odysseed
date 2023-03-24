using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 playerPosition;
    public Level room;
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerPosition = room.initialStartValue;
          //  playerStartStorage.initialStartValue = playerPosition;
            Debug.Log(playerPosition);
            gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            gameManager.buildindex = currentSceneIndex + 1;         
            SceneManager.LoadScene(currentSceneIndex + 1);


        }


    }


}
