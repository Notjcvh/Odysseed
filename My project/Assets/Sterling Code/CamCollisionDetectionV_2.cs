using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CamCollisionDetectionV_2 
{

   static int numberofpoints = 6;
   static readonly Vector3[] viewDirections;


    static CamCollisionDetectionV_2()
    {
        viewDirections = new Vector3[CamCollisionDetectionV_2.numberofpoints];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;  //golden ratio
        float angleIncrement = Mathf.PI * 2 * goldenRatio; // golden angle 

        for (int i = 0; i < numberofpoints; i++)
        {
            float t = (float)i / numberofpoints;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            viewDirections[i] = new Vector3(x, y, z);
        }
    }
}
