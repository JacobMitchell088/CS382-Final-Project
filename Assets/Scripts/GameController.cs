using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnRadius = 10f; // Radius around the player for spawning enemies
    public int maxEnemies = 5; // Maximum number of enemies in the game at once
    public float spawnInterval = 5f; // Time interval between enemy spawns

    private int currentEnemyCount = 0;

    private void Start()
    {
        // Start spawning enemies at intervals
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Find the player GameObject (you can tag your player as "Player" for this to work)
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return; // Exit if player is not found
        }

        // Check if we've reached the maximum number of enemies
        if (currentEnemyCount >= maxEnemies) return;

        // Generate a random position within the spawn radius around the player
        Vector2 randomPos2D = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.transform.position + new Vector3(randomPos2D.x, 0, randomPos2D.y);

        // Instantiate the enemy and increment the enemy count
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }

    // This method should be called by enemies when they are destroyed
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}

