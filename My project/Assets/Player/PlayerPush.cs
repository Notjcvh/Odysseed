using UnityEngine;

public class PlayerPush : MonoBehaviour
{

    public LayerMask pushableLayers;
    public bool canPush;
    [Range(0.5f, 50f)] public float strength = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit Detected");
        if (canPush) PushObject(collision);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down.normalized, Color.green);
    }

    private void PushObject(Collision collision)
    {
        // get Rigidbody
        Rigidbody objectBody = collision.collider.attachedRigidbody;

        // make sure we only push desired layer(s)
        var objectLayerMask = 1 << objectBody.gameObject.layer;
        print("Object Mask : "+ objectLayerMask + " Push Layers Values : " + pushableLayers.value);
        if ((objectLayerMask & pushableLayers.value) == 0) return;


        // We dont want to push objects below us
        RaycastHit hit;
      
        if((Physics.Raycast(transform.position, Vector3.down.normalized * 2f, out hit)))
        {
            if ( hit.normal.y < -0.3f) return;
           
        }

        //Calculate the push Direction
        float pushDirX = ( collision.transform.position.x - this.transform.position.x);
        float pushDirZ = ( collision.transform.position.z - this.transform.position.z);

        Vector3 pushDirection = Vector3.Normalize(new Vector3(pushDirX, 0, pushDirZ));

        Debug.Log(pushDirection);
        // Apply push and take strength into account
        objectBody.AddForce(pushDirection * strength, ForceMode.Impulse);

        // make sure we hit a non kinematic rigidbody
        /* Rigidbody body = hit.collider.attachedRigidbody;
         if (body == null || body.isKinematic) return;*/






    }




}
