using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master_Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public int contactDamage = 10; // Initial hit damage when in contact
    public int damagePerSecond = 10; // Continuous damage if in contact
    public EnemyMovement enemy;
    public GameObject expPrefab; // Reference to the experience prefab
    public GameController gameController;
    private bool isDead;

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyMovement>();
    }

    // This method can be used to apply damage to the enemy
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            DropExperience(); // Call the method to drop experience
            enemy.Die(); // Enemy dies
        }
    }

    private void OnDestroy()
    {
        // Notify GameController when this enemy is destroyed
        if (gameController != null)
        {
            gameController.OnEnemyDestroyed();
        }
    }

    // Method to drop experience
    private void DropExperience()
    {
        if (expPrefab != null)
        {
            Vector3 dropPosition = new Vector3(transform.position.x, 1, transform.position.z);
            // Instantiate the expPrefab at the enemy's position
            Instantiate(expPrefab, dropPosition, Quaternion.identity);
        }
    }
}
