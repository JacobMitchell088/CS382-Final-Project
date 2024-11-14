using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    public float fallSpeed = 30f;       // Speed at which the meteor falls
    public int damage = 50;             // Damage dealt to the enemy on hit
    public float impactRadius = 2f;     // Radius for area of effect aoe damage

    private void Update()
    {
        // Move the meteor downward over time
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void Start() 
    {
        Destroy(gameObject, 5f); // Lifespan of 5 to ensure actor is deleted
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the meteor hit an enemy
        if (other.CompareTag("Enemy"))
        {
            Master_Enemy enemy = other.GetComponent<Master_Enemy>();

            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damage);
            }

            // Destroy the meteor on impact
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If meteor hits the ground, destroy it
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
