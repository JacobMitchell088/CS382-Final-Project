using UnityEngine;

public class BorderRestriction : MonoBehaviour
{
    // Reference to the player's Transform
    public Transform playerPos;

    // Define the boundaries of the level
    public Vector3 minBounds;
    public Vector3 maxBounds;

    // Optional smoothing speed
    public float smoothingSpeed = 10f;

    private void Start()
    {
        // Automatically find the player's Transform if it's not assigned
        if (playerPos == null)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        // Get the current position of the player
        Vector3 currentPosition = playerPos.position;

        // Clamp the position to stay within the boundaries
        float clampedX = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);
        float clampedZ = Mathf.Clamp(currentPosition.z, minBounds.z, maxBounds.z);

        // Apply the clamped position back to the player's Transform
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);

        // Optionally, smooth the transition to the clamped position
        if (currentPosition != clampedPosition)
        {
            playerPos.position = Vector3.Lerp(currentPosition, clampedPosition, smoothingSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a wireframe cube around the boundary
        Gizmos.color = Color.red;
        Vector3 center = (minBounds + maxBounds) / 2;
        Vector3 size = maxBounds - minBounds;
        Gizmos.DrawWireCube(center, size);
    }
}
