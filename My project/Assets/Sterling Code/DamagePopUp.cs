using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{

    private TextMeshPro textMesh;
     

  public static DamagePopUp Create(Vector3 position, int damageAmount)
    {
       
        Transform  damagePopupTransfrom = Instantiate(GameAssets.i.DamageTextHolder, position, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopupTransfrom.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount);
        
        return damagePopUp;
    }

   

    //Function to convert integer inputs into strings 
    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
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
    }
}
