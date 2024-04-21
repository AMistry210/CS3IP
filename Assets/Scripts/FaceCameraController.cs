using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FaceCameraController : MonoBehaviour
{
    // Reference to the FaceTracker script
    public FaceTracker faceTracker;

    // Reference to the spaceship's Rigidbody component
    public Rigidbody spaceshipRigidbody;

    // Reference to the Cinemachine virtual camera
    public CinemachineVirtualCamera virtualCamera;

    // Speed at which the spaceship follows the face
    public float followSpeed = 5f;

    // Smooth damping parameters for camera movement
    public float smoothTime = 0.3f;
    private Vector3 cameraVelocity = Vector3.zero; // Velocity for camera smoothing

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the FaceTracker script is assigned and the face position is not null
        if (faceTracker != null && faceTracker.facePosition != Vector2.zero)
        {
            // Get the position of the detected face from the FaceTracker script
            Vector2 facePosition = faceTracker.facePosition;

            // Convert face position to world space
            Vector3 targetPosition = new Vector3(facePosition.x, facePosition.y, spaceshipRigidbody.position.z);

            // Move the spaceship towards the face position
            spaceshipRigidbody.MovePosition(Vector3.Lerp(spaceshipRigidbody.position, targetPosition, Time.fixedDeltaTime * followSpeed));

            // Smoothly move the virtual camera towards the face position
            Vector3 cameraTargetPosition = new Vector3(facePosition.x, facePosition.y, virtualCamera.transform.position.z);
            virtualCamera.transform.position = Vector3.SmoothDamp(virtualCamera.transform.position, cameraTargetPosition, ref cameraVelocity, smoothTime);
        }
    }
}

