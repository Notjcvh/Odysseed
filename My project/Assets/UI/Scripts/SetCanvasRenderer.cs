using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasRenderer : MonoBehaviour
{
    public GameManager gameManager;
    public Camera myCamera;
    public Canvas canvas;
    public int order;

    public virtual void OnEnable()
    {
        gameManager = GetComponentInParent<GameManager>();
        canvas = GetComponent<Canvas>();
        myCamera = Camera.main;
        canvas.worldCamera = myCamera;
        canvas.planeDistance = .15f;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = order;
    }
}
