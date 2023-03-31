using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float secondsTillDestroy;

    private void Start()
    {
        Destroy(this.gameObject, secondsTillDestroy);
    }
}
