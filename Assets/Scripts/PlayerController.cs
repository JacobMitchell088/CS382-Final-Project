using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Master_Enemy enemy = collision.gameObject.GetComponent<Master_Enemy>();
        if (enemy != null)
        {
            enemy.StartDamageOverTime(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Master_Enemy enemy = collision.gameObject.GetComponent<Master_Enemy>();
        if (enemy != null)
        {
            enemy.StopDamageOverTime();
        }
    }

    // Method for the enemy to call when applying damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement death behavior (e.g., respawn, game over screen, etc.)
    }
}
