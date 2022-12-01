using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 playerPosition;
    public VectorValue room;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerPosition = room.initialStartValue;
          //  playerStartStorage.initialStartValue = playerPosition;
            Debug.Log(playerPosition);
            SceneManager.LoadScene(room.sceneName);
        }


    }


}
