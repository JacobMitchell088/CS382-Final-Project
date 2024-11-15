using UnityEngine;

public class RotateOnYAxis : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    void Update()
    {
        // Get the current rotation of the GameObject
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Rotate only on the Y-axis
        float newYRotation = currentRotation.y + rotationSpeed * Time.deltaTime;

        // Set the new rotation, preserving the original X and Z rotation
        transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
    }
}
