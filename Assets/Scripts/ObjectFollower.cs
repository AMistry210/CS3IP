using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    // Reference to the camera to follow
    public Transform target;

    // Offset distance from the camera
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Adjust as needed

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Update the position of the object to match the camera's position with offset
            transform.position = target.position + offset;
        }
    }
}