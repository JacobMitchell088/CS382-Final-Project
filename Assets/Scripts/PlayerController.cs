using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for using UI components

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Reference to the health bar UI slider
    private float lastDamageTime;

    private bool isDead = false; // To check if the player is dead
    private Rigidbody rb; // Player's Rigidbody for movement control (optional, if needed)

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>(); // Get the player's Rigidbody if needed

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Master_Enemy enemy = other.GetComponent<Master_Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Player collided with enemy, taking initial damage.");
                // Take initial contact damage
                TakeDamage(enemy.contactDamage);
                lastDamageTime = Time.time; // Record the time of initial trigger enter
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Master_Enemy enemy = other.GetComponent<Master_Enemy>();
            if (enemy != null)
            {
                // Check if 1 second has passed since last damage
                if (Time.time - lastDamageTime >= 1.0f)
                {
                    //Debug.Log("Player in contact with enemy, taking damage per second.");
                    // Take the enemy's specific damage per second
                    TakeDamage(enemy.damagePerSecond);
                    lastDamageTime = Time.time; // Update the last damage time
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Master_Enemy enemy = other.GetComponent<Master_Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Player exited contact with enemy.");
                // Reset the last damage time when exiting trigger
                lastDamageTime = 0;
            }
        }
    }
    public void TakeDamage(int amount)
    {
        if (isDead) return; // If the player is dead, don't take damage

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // Prevent health from going below 0
        Debug.Log("Player took " + amount + " damage. Current health: " + currentHealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true; // Set the death flag

        Debug.Log("Player has died.");

        // Play death animation or trigger game over
        // Example: Play the death animation here if you have one
        // animator.SetTrigger("Die");  // Uncomment if you have an Animator

        // Disable movement (optional)
        if (rb != null)
        {
            rb.isKinematic = true; // Disable Rigidbody-based movement
        }
    }
}
