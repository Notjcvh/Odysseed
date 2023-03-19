using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 0f;

    public bool ForwardX = false;
    public bool ForwardY = false;
    public bool ForwardZ = false;

    public bool ReverseX = false;
    public bool ReverseY = false;
    public bool ReverseZ = false;

    void Update()
    {
        float s = Time.deltaTime * speed;

        if (ForwardX) transform.Rotate(s, 0, 0, Space.Self);
        if (ForwardY) transform.Rotate(0, s, 0, Space.Self);
        if (ForwardZ) transform.Rotate(0, 0, s, Space.Self);

        if (ReverseX) transform.Rotate(-s, 0, 0, Space.Self);
        if (ReverseY) transform.Rotate(0, -s, 0, Space.Self);
        if (ReverseZ) transform.Rotate(0, 0, -s, Space.Self);
    }
}