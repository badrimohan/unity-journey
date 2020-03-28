using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    [SerializeField]
    private Vector3 distanceFromCharacter;
    private Vector3 prevCharacterPosition;
    private Vector3 currCharacterPosition;
    private Vector3 positionDelta;
    private float rotateHorizontal;
    private float rotateVertical;
    [SerializeField]
    private float Sensitivity = 2.0f;
    private Vector3 prevDistanceFromCharacter;
    [SerializeField]
    private float maxVRotationAngle = 45.0f;
    [SerializeField]
    private float minVRotationAngle = 0.0f;
    private float vRotateAmount;
    private float inputRotationAngle;
    private float vRotationThreshold = 90.0f;
    private float currentRotationX;
    private Vector3 currCameraPosition;
    private Vector3 targetCameraPosition;
    [SerializeField]
    private float smoothFactor = 0.3f;
    private Vector3 velocity = Vector3.zero;
    
    private void Start() {
        prevCharacterPosition = character.transform.position;
        // UpdateCamDistance(distanceFromCharacter, prevCharacterPosition);
        // gameObject.transform.position = prevCharacterPosition - distanceFromCharacter;
        prevDistanceFromCharacter = distanceFromCharacter;

        // Keep minimum vertical rotation positive.
        if (minVRotationAngle < 0) {
            minVRotationAngle = 0;
        }
    }

    // Update the camera position based on the amount the character has moved.
    Vector3 UpdateCamPosition(Vector3 prevCharacterPosition) {

        // Obtain current position of the character.
        currCharacterPosition = character.transform.position;

        // Calculate the difference between camera's current position 
        // and the character's position
        positionDelta = currCharacterPosition - prevCharacterPosition;

        // Move the camera by the same amount the character has moved.
        // gameObject.transform.Translate(positionDelta, Space.World);
        
        //Experimental
        currCameraPosition = gameObject.transform.position;
        targetCameraPosition = currCameraPosition + positionDelta;
        // gameObject.transform.position = Vector3.SmoothDamp(currCameraPosition, targetCameraPosition, ref velocity, smoothTime);
        gameObject.transform.position = Vector3.Lerp(currCameraPosition, targetCameraPosition, Time.deltaTime * smoothFactor);

        return currCharacterPosition;
    }

    void UpdateCamDistanceFromCharacter() {
        if (distanceFromCharacter != prevDistanceFromCharacter) {
            positionDelta += distanceFromCharacter - prevDistanceFromCharacter;
            prevDistanceFromCharacter = distanceFromCharacter;
            gameObject.transform.Translate(positionDelta, Space.Self);
        }
    }

    // Rotates the camera around character's current position based on axis input.
    // PS4 - Right Analog Stick
    void UpdateCamRotation(Vector3 characterPosition) {
        
        // Obtain Axes input.
        rotateHorizontal = Input.GetAxis("RightStickX");
        rotateVertical = Input.GetAxis("RightStickY");

        // Rotate camera about the character's current position based on horizontal input.
        // gameObject.transform.RotateAround(characterPosition, Vector3.up, rotateHorizontal * Sensitivity);
        transform.position = MyRotateAround(transform.position, characterPosition, Vector3.up, rotateHorizontal * Sensitivity);
        // transform.LookAt(characterPosition, Vector3.up);

        // Limit the rotation on the vertical input based on minVRotationAngle and maxVRotationAngle
        // So, if the values were 0 and 45, the camera only rotates wihin those angles.
        currentRotationX = gameObject.transform.rotation.eulerAngles.x;
        inputRotationAngle = rotateVertical * Sensitivity + currentRotationX;
        if (inputRotationAngle > maxVRotationAngle && inputRotationAngle < vRotationThreshold) {
            vRotateAmount = maxVRotationAngle - currentRotationX;
        } else if (minVRotationAngle >= 0 && inputRotationAngle < minVRotationAngle) {
            vRotateAmount = minVRotationAngle - currentRotationX;
        } else {
            vRotateAmount = rotateVertical * Sensitivity;
        }
        gameObject.transform.RotateAround(characterPosition, transform.right, vRotateAmount);

    }

    Vector3 MyRotateAround(Vector3 position, Vector3 point, Vector3 axis, float angle) {
        if (angle == 0) {
            return position;
        }
        Vector3 result;
        float distance = Vector3.Distance(position, point);
        float yDistance = position.y - point.y;
        float radius = Mathf.Sqrt(Mathf.Pow(distance, 2) - Mathf.Pow(yDistance, 2));

        result.x = position.x + radius * Mathf.Cos(Mathf.Rad2Deg * angle);
        result.y = 0;
        result.z = position.z + radius * Mathf.Sin(Mathf.Rad2Deg * angle);

        return result;
    }
    void Update()
    {
        // Update Camera Position
        currCharacterPosition = UpdateCamPosition(prevCharacterPosition);
        // Store current frame's character position
        prevCharacterPosition = currCharacterPosition;
        // Update camera position if changes are made to distance.
        UpdateCamDistanceFromCharacter();
    }
    void LateUpdate() {
        UpdateCamRotation(currCharacterPosition);
    }
}
