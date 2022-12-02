using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{

    private TextMeshPro textMesh;
    private Color textColor;
    private static int sortingOrder;
     

    [Header("Testing Values")]
    public float testDisappearTimer = 1f;
    public float disappearSpeed = 3f;


    public static DamagePopUp Create(Vector3 position, int damageAmount)
    {
        Vector3 offfset = new Vector3( 0,.5f,0);
        Vector3   newPos = position + offfset;
       
        Transform  damagePopupTransfrom = Instantiate(GameAssets.i.DamageTextHolder, newPos , Quaternion.identity);
        DamagePopUp damagePopUp = damagePopupTransfrom.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount);
        
        return damagePopUp;
    }

   

    //Function to convert integer inputs into strings 
    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        //grab color of our Text mesh 
        textColor = textMesh.color;
        sortingOrder++;
        //make sure the first text popup is always ontop
        textMesh.sortingOrder = sortingOrder;
    }


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }


    //Makes sure the text is always facing the camera 
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

        //Text movement over time
        float speed = 10f;
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;

        testDisappearTimer -= Time.deltaTime;

        if(testDisappearTimer < 0 )
        {
            // start disappearing change alpha then destory
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
                Destroy(gameObject);
        }

    }
}
