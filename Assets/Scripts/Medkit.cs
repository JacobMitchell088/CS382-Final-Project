using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healAmount = -20; // Negative since we use the TakeDamage function to adjust health
    private int rotationSpeed = 125;



    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(healAmount);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Player Controller Is Null");
        }
    }

    private void Update()
    {
        Vector3 rotationAxis = new Vector3(0, 1, 0).normalized;
            
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
