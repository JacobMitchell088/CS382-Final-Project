using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Impact
    public GameObject impactEffectPrefab; // VFX Prefab
    public Color flashColor = Color.red; // Color to flash on impact
    public float flashDuration = 0.1f;
    private Color originalColor;
    private Renderer enemyRenderer;

    // Health bar
    public GameObject healthBarPrefab; // Reference to the health bar prefab
    private Slider healthBarSlider; // The Slider component for health bar
    private Camera mainCamera; // Reference to the main camera
    private Vector3 healthBarOffset = new Vector3(0, 2f, 0); // Offset for health bar position above the enemy
    private GameObject healthBarObject; // To store the reference to the health bar GameObject

    // Medkit
    public GameObject hpMedkitPrefab; // Assign medkit prefab
    public float hpMedkitSpawnChance = 0.01f;

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyMovement>();

        // Color flash setup
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
        else
        {
            Debug.Log("No Renderer On Enemy");
        }

        // Create and assign the health bar slider
        if (healthBarPrefab != null)
        {
            // Use FindObjectOfType<Canvas>() instead of GameObject.Find("Canvas")
            Canvas canvas = FindObjectOfType<Canvas>();
            
            if (canvas != null)
            {
                // Instantiate the health bar prefab and set its parent to the canvas
                healthBarObject = Instantiate(healthBarPrefab, transform);
                //healthBarObject.transform.SetParent(canvas.transform, false); // Set the parent to the canvas

                // Try to find the Slider component in the instantiated health bar object
                healthBarSlider = healthBarObject.GetComponentInChildren<Slider>();

                // Null check for healthBarSlider before using it
                if (healthBarSlider != null)
                {
                    // Set the health bar values
                    healthBarSlider.maxValue = maxHealth; // Set max health
                    healthBarSlider.value = currentHealth; // Set initial health value
                }
                else
                {
                    Debug.LogError("Health Bar Prefab does not have a Slider component in its children.");
                }
            }
            else
            {
                Debug.LogError("Canvas not found in the scene. Please make sure there's a Canvas in the scene.");
            }
        }
        else
        {
            Debug.LogError("Health Bar Prefab is not assigned in the Inspector.");
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (healthBarSlider != null)
        {
            UpdateHealthBarPosition();
        }
        
    }

    // Update the health bar's position in camera space
    private void UpdateHealthBarPosition()
    {
        if (healthBarSlider != null && mainCamera != null)
        {
            // Position the health bar above the enemy in world space
            Vector3 worldPosition = transform.position + healthBarOffset;

            // Convert the world position to screen space
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            // Set the position of the health bar on the canvas in screen space
            healthBarSlider.transform.position = screenPosition;
        }
    }

    // This method can be used to apply damage to the enemy
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, 1f); // Destroy the VFX after 1s
        }

        StartCoroutine(FlashDamage());


        if (healthBarSlider != null) 
        {
            healthBarSlider.value = currentHealth; // Update the health bar value
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die(); // Call the death method when health reaches zero
        }
    }

    private void OnDestroy()
    {
        // Notify GameController when this enemy is destroyed
        if (gameController != null)
        {
            gameController.OnEnemyDestroyed();
        }

        // Make sure to clean up health bar if it exists
        if (healthBarObject != null)
        {
            Destroy(healthBarObject); 
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

    // Flash Damage
    private IEnumerator FlashDamage()
    {
        // Change to flash color
        enemyRenderer.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        enemyRenderer.material.color = originalColor;
    }

    // Handle the enemy's death
    private void Die()
    {
        isDead = true;
        DropExperience(); // Call the method to drop experience
        TrySpawnHPMedkit(); // Have a chance to spawn a medkit
        enemy.Die(); // Enemy dies

        // Destroy the health bar after the enemy dies
        if (healthBarObject != null)
        {
            Destroy(healthBarObject);
        }
    }

    private void TrySpawnHPMedkit()
    {
        if (Random.value <= hpMedkitSpawnChance && hpMedkitPrefab != null)
        {
            Instantiate(hpMedkitPrefab, transform.position, Quaternion.identity);
        }
    }
}
