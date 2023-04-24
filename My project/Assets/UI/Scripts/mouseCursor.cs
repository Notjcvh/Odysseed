using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultCursor;
    public Sprite selectedCursor;

    public Vector3 screenPos;
    public Vector3 worldPos;
    public Vector3 offset;
    public float depth = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {





        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + depth; //create forwarf depth
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos + offset;
       

      



        if (Input.GetMouseButton(0))
        {

            spriteRenderer.sprite = selectedCursor;
        }
        else
        {
            spriteRenderer.sprite = defaultCursor;
        }    


    }
}
