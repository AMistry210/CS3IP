using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using System.Collections;

public class FaceTracker : MonoBehaviour
{
    private CascadeClassifier cascadeClassifier;
    private WebCamTexture webCamTexture;

    // Public property to expose the face position
    public Vector2 facePosition { get; private set; }

    // Camera movement speed
    public float movementSpeed = 0.05f; // Decreased movement speed

    // Frequency of face detection updates (in seconds)
    public float detectionFrequency = 0.1f;

    // Reference to the target position for the camera
    public Transform cameraTarget;

    // Layer mask for collision detection
    public LayerMask collisionLayerMask;

    private void Start()
    {
        cascadeClassifier = new CascadeClassifier();
        string cascadeFilePath = Application.streamingAssetsPath + "/haarcascade_frontalface_alt.xml";

        // Log the file path for debugging purposes
        Debug.Log("Cascade classifier file path: " + cascadeFilePath);

        try
        {
            if (cascadeClassifier.load(cascadeFilePath))
            {
                Debug.Log("Cascade classifier loaded successfully.");
            }
            else
            {
                throw new System.Exception("Failed to load cascade classifier.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading cascade classifier: " + e.Message);
            return;
        }

        // Start the webcam feed
        webCamTexture = new WebCamTexture();
        if (!webCamTexture.isPlaying)
        {
            webCamTexture.Play();
        }
        else
        {
            Debug.LogError("Failed to start webcam texture.");
        }

        // Start face detection coroutine
        StartCoroutine(DetectFaces());
    }

    private IEnumerator DetectFaces()
    {
        while (true)
        {
            yield return new WaitForSeconds(detectionFrequency);

            if (webCamTexture == null || !webCamTexture.isPlaying)
            {
                Debug.LogWarning("Webcam texture is not ready or not playing.");
                continue;
            }

            // Convert the webcam texture to Mat
            Mat frame = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
            Utils.webCamTextureToMat(webCamTexture, frame);

            // Detect faces in the frame
            MatOfRect faces = new MatOfRect();
            cascadeClassifier.detectMultiScale(frame, faces);

            // Check if at least one face is detected
            if (faces.total() > 0)
            {
                // Get the first detected face
                OpenCVForUnity.CoreModule.Rect faceRect = faces.toArray()[0];

                // Calculate the center of the detected face
                float faceCenterX = faceRect.x + faceRect.width / 2f;
                float faceCenterY = webCamTexture.height - (faceRect.y + faceRect.height / 2f); // Adjust Y-coordinate

                // Update the face position
                facePosition = new Vector2(faceCenterX, faceCenterY);

                // Perform raycast to detect collisions with objects on the "nopasslayer"
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, collisionLayerMask))
                {
                    // Check if the raycast hit one of the transparent planes
                    if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("nopasslayer"))
                    {
                        // Get the target position from the hit
                        Vector3 targetPosition = hit.point;

                        // Smoothly move the camera target towards the target position
                        cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPosition, movementSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                // If no face is detected, reset the face position
                facePosition = Vector2.zero;
            }
        }
    }
}