using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Make sure to change in editor not here
    private Camera mainCamera;   

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        MovePlayer();
        //RotateToMouse(); // Will spin camera until we make rotation relative to world rotation & not parent rot
    }

    void MovePlayer() {

        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveZ = Input.GetAxisRaw("Vertical");   

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void RotateToMouse() { // Need to find out how to make rotation based on world rotation rather than parent rotation in unity
        // Get the mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y; // Keep rot level on the y axis

            // Rotate the player to face the mouse 
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
