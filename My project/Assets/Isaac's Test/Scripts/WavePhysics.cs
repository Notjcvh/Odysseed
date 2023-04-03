using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePhysics : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction;
    public Transform player;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction;
    }
}
