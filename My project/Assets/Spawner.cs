using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    void SpawnObj();
}




public class Spawner : MonoBehaviour, ISpawnable
{
    public GameObject obj;

    public void SpawnObj()
    {
        Instantiate(obj, this.transform.position, this.transform.rotation, this.transform);
    }
}


