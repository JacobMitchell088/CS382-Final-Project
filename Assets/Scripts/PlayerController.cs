using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for using UI components
using TMPro; // Required for TextMeshPro

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Reference to the health bar UI slider
    public TextMeshProUGUI healthText; // Reference to the health text display
    public AudioSource hitSound;

    private float lastDamageTime;
    private bool isDead = false; // To check if the player is dead
    private Rigidbody rb; // Player's Rigidbody for movement control (optional, if needed)
    public ExpController expController; // Reference to the ExpController to handle experience

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>(); // Get the player's Rigidbody if needed
        hitSound = GetComponent<AudioSource>();

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Update the initial health text
        UpdateHealthText();
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
        // Check if the player collides with an enemy
        if (other.CompareTag("Enemy"))
        {
            Master_Enemy enemy = other.GetComponent<Master_Enemy>();
            if (enemy != null)
            {
                // Take initial contact damage
                TakeDamage(enemy.contactDamage);
                hitSound.Play();
                lastDamageTime = Time.time; // Record the time of initial trigger enter
            }
        }
        
        // Check if the player collides with an experience object
        if (other.CompareTag("EXP"))
        {
            int expAmount = other.GetComponent<ExpItem>().expValue;

            // Add experience to the player's ExpController
            if (expController != null)
            {
                expController.AddExperience(expAmount);
            }

            // Destroy the experience object after it's picked up
            Destroy(other.gameObject);
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
                    // Take the enemy's specific damage per second
                    TakeDamage(enemy.damagePerSecond);
                    hitSound.Play();
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            lastDamageTime = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player took " + amount + " damage. Current health: " + currentHealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Update the health text
        UpdateHealthText();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
