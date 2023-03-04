using UnityEngine;

public class PlayerPush : MonoBehaviour
{

    public LayerMask pushableLayers;
    [Range(0.5f, 50f)] public float strength = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == pushableLayers) PushObject(collision);
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

        Vector3 direction = collision.contacts[0].point - transform.position;
        direction = -direction.normalized;

        // float pushDirX = ( collision.transform.position.x - this.transform.position.x);
        //float pushDirZ = ( collision.transform.position.z - this.transform.position.z);

        // Vector3 pushDirection = Vector3.Normalize(new Vector3(pushDirX, 0, pushDirZ));

        // Apply push and take strength into account
        objectBody.AddForce(direction * strength,ForceMode.Force);

        // make sure we hit a non kinematic rigidbody
        /* Rigidbody body = hit.collider.attachedRigidbody;
         if (body == null || body.isKinematic) return;*/
    }




}
