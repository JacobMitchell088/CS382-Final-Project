using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public int contactDamage = 10; // Initial hit damage when in contact
    public float damagePerSecond = 5f; // Continuous damage if in contact
    public EnemyMovement enemy;
    private bool isTouchingPlayer = false; // Flag to check if enemy is touching player



    private void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (isTouchingPlayer)
        {
            // Continuously apply damage over time while touching the player
            TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    // This method can be used to apply damage to the enemy
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            enemy.Die();
        }
    }
    
    // Use OnTriggerEnter or OnCollisionEnter to detect contact damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming the player has a "Player" tag
        {
            isTouchingPlayer = true;
            // Apply immediate contact damage when player enters the collider
            other.gameObject.GetComponent<PlayerController>().TakeDamage(contactDamage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
}
