using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : MonoBehaviour
{
    public float speed = 10f;               // Speed of the bullet
    public float maxDistance = 15f;         // Maximum distance before returning
    public int damage = 25;                 // Damage the bullet deals to enemies
    public int yHeight = 1;                 // Locked Y Height 

    private Vector3 startPoint;             // Point where the bullet was instantiated
    private bool returning = false;         // Whether the bullet is returning to the player
    public Transform player;               // Reference to the player's position

    private void Start()
    {
        startPoint = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    private void Update()
    {
        // Lock y position to specific height
        Vector3 position = transform.position;
        position.y = yHeight;
        transform.position = position;


        // Check if the bullet has reached the max distance
        if (!returning && Vector3.Distance(startPoint, transform.position) >= maxDistance)
        {
            returning = true; // Start returning to the player
        }

        // Move the bullet
        if (returning)
        {
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // Move outward in the forward direction
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit an enemy
        Master_Enemy enemy = other.GetComponent<Master_Enemy>();
        
        if (enemy != null)
        {
            // Damage the enemy
            enemy.TakeDamage(damage);
        }
        else if (other.CompareTag("Player"))
        {
            if (returning)
                Destroy(gameObject);
        }

        // Optional: Destroy the bullet if it hits an enemy
        // Destroy(gameObject);
    }
}
