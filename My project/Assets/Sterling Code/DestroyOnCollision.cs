using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public string tagToDestroy;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(tagToDestroy))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        
    }
}
