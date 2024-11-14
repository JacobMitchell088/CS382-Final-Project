using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for using UI components

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public Slider healthBar; // Reference to the health bar UI slider

    private float damageCooldown = 1.0f; // Damage interval in seconds
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

    public void TakeDamage(float amount)
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

        // Disable player controls or other necessary systems
        // Example: Disable all controls or UI updates
        // GetComponent<PlayerMovement>().enabled = false; // Disable movement script
        // GetComponent<PlayerCombat>().enabled = false; // Disable combat script
    }
}
