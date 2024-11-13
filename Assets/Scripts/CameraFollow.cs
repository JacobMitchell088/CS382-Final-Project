using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Reference to the player's transform
    public float followSpeed = 5f;  // Speed at which the camera follows the player
    public float height = 10f;      // The height at which the camera should follow the player

    private Vector3 offset;         // Offset from the player's position (in the X and Z axis)

    void Start()
    {
        // Set the initial offset relative to the player's position on X and Z axes
        offset = new Vector3(transform.position.x - player.position.x, 0, transform.position.z - player.position.z);
    }

    void Update()
    {
        // Set the new target position for the camera on the X and Z axes
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, height, player.position.z + offset.z);

        // Smoothly move the camera to the target position using Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
