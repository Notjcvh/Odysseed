using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour, ISpawnable
{
    
    public PlayerManger playerManger;
    public GameObject heart;
    public Transform points;
    public bool called;
    public GameObject spawnedObj; 
    public float spawnPorbability = 0.5f; // 20%
    public Vector3 offset;


    void Start()
    {
        playerManger = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManger>();
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (playerManger.currentHealth <= 3 && other.gameObject.tag == ("Player"))
        {
            if (Random.value > spawnPorbability && spawnedObj == null)
            {
                StartCoroutine(Delay());
                Debug.Log("true");

            }
            else
            {
                Debug.Log("false");
            }
        }
    }

    IEnumerator Delay()
    {
        SpawnObj();
        yield return new WaitForSecondsRealtime(4);
    }        
    
    public void SpawnObj()
    {
        int childCount = points.childCount;
        Transform randomChild;
        Debug.Log(childCount);
        if (childCount > 0)
        {
            int randomIndex = Random.Range(0, childCount);
            randomChild = transform.GetChild(randomIndex);
            Debug.Log(randomChild);
        }
        else
        {
            //No child found use the parent transform
            randomChild = this.transform;
        }

        spawnedObj = Instantiate(heart, randomChild.position + offset, randomChild.rotation, this.transform);
        Rigidbody rb = spawnedObj.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }
}

