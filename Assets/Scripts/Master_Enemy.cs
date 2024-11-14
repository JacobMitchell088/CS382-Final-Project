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



    private void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyMovement>();
    }

    // This method can be used to apply damage to the enemy
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            enemy.Die();
        }
    }
    
}
