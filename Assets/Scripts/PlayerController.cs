using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for using UI components

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Reference to the health bar UI slider

    private float damageCooldown = 1.0f; // Damage interval in seconds
    private float lastDamageTime;
    
    private void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Master_Enemy enemy = collision.gameObject.GetComponent<Master_Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.contactDamage);
            lastDamageTime = Time.time;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Master_Enemy enemy = collision.gameObject.GetComponent<Master_Enemy>();
        if (enemy != null)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(enemy.damagePerSecond);
                lastDamageTime = Time.time;
            }
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took " + amount + " damage. Current health: " + currentHealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
