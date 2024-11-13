using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int contactDamage = 10; // Initial hit damage
    public int damagePerSecond = 5; // Continuous damage if in contact

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // This method can be used to apply damage to the enemy
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implement the enemy death logic (e.g., destroy the enemy, play animation, etc.)
        Destroy(gameObject);
    }
}
