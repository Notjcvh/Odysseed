using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent : MonoBehaviour
{
    public Transform gun;
    public Transform spawner;
    public GameObject projectilePrefab;

    public float projectileSpeed; // in meters per second 
    public float secondsPerLaunch;
    public float secondsEllapsed;

    private void Start()
    {
        float rotY = Random.Range(-180, 180);
        
        //Rotating the gun around the y axis at random 
        // gun.rotation = Quaternion.AngleAxis(rotY, Vector3.up);
        gun.rotation = Quaternion.Euler(0, rotY, 0);

        //Spawning a fireball projectile
        
    }


    // Update is called once per frame
    void Update()
    {
        secondsEllapsed += Time.deltaTime; //used for tracking how much time has ellapsed between frames


        if(secondsEllapsed >= secondsPerLaunch)
        {
            //Spawn fireball
            GameObject projectile = Instantiate(projectilePrefab, spawner.position, spawner.transform.rotation);
            projectile.GetComponent<Rigidbody>().velocity = spawner.forward * projectileSpeed;

            secondsEllapsed = 0;
        }


    }
}
