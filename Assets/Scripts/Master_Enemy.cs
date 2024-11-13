using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_Enemy : MonoBehaviour
{
    public int damage = 10;
    public float dps = 1.0f; // Damage interval in seconds

    private PlayerController player;
    private float damageTimer = 0f;
    private bool isTouchingPlayer = false;

    // Stats
    public float maxHealth = 100f;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isTouchingPlayer)
        {
            // Increment the timer
            damageTimer += Time.deltaTime;

            // Apply damage if the timer has reached the interval
            if (damageTimer >= dps)
            {
                player.TakeDamage(damage);
                damageTimer = 0f; // Reset timer after damage is applied
            }
        }
    }

    public void StartDamageOverTime(PlayerController player)
    {
        this.player = player;
        isTouchingPlayer = true;
        player.TakeDamage(damage); // Initial damage on contact
        damageTimer = 0f; // Reset the timer for ongoing damage
    }

    public void StopDamageOverTime()
    {
        isTouchingPlayer = false;
        damageTimer = 0f;
    }

    // Method for bullets / objects to call to deal damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
