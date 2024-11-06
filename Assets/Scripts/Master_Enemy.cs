using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;           // Enemy movement speed
    public float attackRange = 1.5f;       // Distance within which the enemy will attack
    public float attackCooldown = 2f;      // Cd time between attacks

    private Transform player;              // Reference to the player's transform
    private float lastAttackTime = 0f;     // Track the last attack time

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() 
    {
        if (player != null) {
            MoveTowardsPlayer();
            TryAttackPlayer();
        }
    }


    void MoveTowardsPlayer() {
        // Calculate direction to player and move towards them
        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange) {
            // Move towards the player
            Vector3 targetPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            targetPosition.y = 0f; // Ensure the enemy stays at y = 0
            transform.position = targetPosition;

            // Rotate enemy to face player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    void TryAttackPlayer() {

        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown) {
            AttackPlayer();
            lastAttackTime = Time.time; // Reset attack timer
        }
    }

    void AttackPlayer() { // TODO
        
        Debug.Log("Enemy hits player");
    }
}
