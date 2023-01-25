using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionView : MonoBehaviour
{
    public Material material;
    [Range(0, 1)] public float alphaValue;
    public Color  originalColor, alphaColor;
    public float rateOfChange= 3f;

    private bool obstructing = false;
    private void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        originalColor = material.color;
        alphaColor = material.color;
        alphaColor.a = alphaValue;
    }



    void Obstructing()
    {
        material.color = Color.Lerp(originalColor, alphaColor, rateOfChange);
    }

    void NotObstructing()
    {
        originalColor.a = 1;
        material.color = Color.Lerp(alphaColor, originalColor, rateOfChange);


    }
}
