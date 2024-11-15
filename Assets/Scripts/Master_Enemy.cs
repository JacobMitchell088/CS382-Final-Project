using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    // Impact
    public GameObject impactEffectPrefab; // VFX Prefab
    public Color flashColor = Color.red; // Color to flash on impact
    public float flashDuration = 0.1f;
    private Color originalColor;
    private Renderer enemyRenderer;

    // HP Bar
    public GameObject healthBarPrefab;      // The prefab of the health bar
    public float healthBarOffsetY = 5.0f;   // Height of HP bar

    private Transform healthBarTransform;   // Position where the health bar should appear
    private GameObject healthBarInstance;   // Instance of the health bar
    private Image healthBarFill;            // Reference to the fill image
    private Camera mainCamera;              // Reference to camera




    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        enemy = GetComponent<EnemyMovement>();

        // Color flash
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null) 
        {
        originalColor = enemyRenderer.material.color;
        }
        else
        {
            Debug.Log("No Renderer On Enemy");
        }

        // Instantiate the health bar prefab and set it above the enemy
        healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        
        healthBarTransform = healthBarInstance.transform;
        
        // Get reference to the fill image (assuming it's the first child of the health bar)
        healthBarFill = healthBarInstance.transform.GetChild(0).GetComponent<Image>();


        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Update the health bar position above the enemy
        SetHealthBarPosition();

        // Update health bar fill
        healthBarFill.fillAmount = currentHealth / maxHealth;
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



        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            DropExperience(); // Call the method to drop experience
            Destroy(healthBarInstance);
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
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
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

    private void SetHealthBarPosition()
    {
        Vector3 enemyPosition = transform.position;

        // Add an offset on the Y axis (above the enemy)
        Vector3 adjustedPosition = enemyPosition + new Vector3(0, healthBarOffsetY, 0);

        // Convert world position to screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(adjustedPosition);

        healthBarInstance.transform.position = screenPosition;
        // Check if the screen position is valid (not behind the camera)
        if (screenPosition.z > 0)
        {
            // Position the health bar at the screen position, relative to the canvas
            //healthBarInstance.transform.position = screenPosition;
        }
        else
        {
            // Optionally, handle the case where the enemy is off-screen (e.g., hide health bar)
            //healthBarInstance.SetActive(false);
        }
    }

}
