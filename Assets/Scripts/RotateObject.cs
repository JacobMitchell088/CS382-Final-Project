using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Speed of rotation (degrees per second)
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Get the current rotation
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Only update the Y-axis, keep X and Z unchanged
        float newYRotation = currentRotation.y + rotationSpeed * Time.deltaTime;

        // Apply the new rotation while keeping X and Z the same
        transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
    }
}
