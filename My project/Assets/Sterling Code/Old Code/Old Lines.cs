using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLines : MonoBehaviour
{
    /* For CameraContorller changes camera for combat
     * 
    [SerializeField] private float newMaxDistance;
    [SerializeField] private float newVerticalAngle;
    public void CombatCam(){
      float mouseX = Input.GetAxisRaw("Mouse X");
      targetDistance = Mathf.Clamp(newMaxDistance, newMaxDistance, newMaxDistance);
      plannerDirection = Quaternion.Euler(0, mouseX, 0) * plannerDirection;
      targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, newVerticalAngle, newVerticalAngle);
      newRotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotationSharpness * Time.deltaTime);
      newPosition = Vector3.Lerp(cam.transform.position, targetPosition, rotationSharpness * Time.deltaTime);
      targetRotation = Quaternion.LookRotation(plannerDirection) * Quaternion.Euler(targetVerticalAngle, 0, 0);
      targetPosition = followObj.position - (targetRotation * Vector3.forward) * targetDistance;
      cam.transform.rotation = newRotation;
      cam.transform.position = newPosition; }
    */









}
