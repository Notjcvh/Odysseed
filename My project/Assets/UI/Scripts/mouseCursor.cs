using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{

    public PlayerInput playerInput;
    public GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    public Sprite defaultCursor;
    public Sprite selectedCursor;
    public LayerMask layer;

    private Vector3 screenPos;
    private Vector3 worldPos;
    public Vector3 offset;
    public float depth = 1;

    // Start is called before the first frame update
    void OnEnable()
    {
       // playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GetComponentInParent<GameManager>();
        spriteRenderer.sortingLayerName = "UI";
        spriteRenderer.sortingOrder = 5;
     
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        screenPos = Input.mousePosition + offset * 4f;
        screenPos.z = Camera.main.nearClipPlane + depth; //create forwarf depth
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = worldPos; //+ offset;


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
