using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of different enemy prefabs (tiers)
    public float spawnRadius = 10f; // Radius around the player for spawning enemies
    public int initialEnemiesPerRound = 10; // Initial number of enemies to spawn in the first round
    public float spawnInterval = 5f; // Time interval between enemy spawns

    private int currentRound = 1;
    private int enemiesToSpawn;
    private int enemiesRemaining; // Enemies left to be defeated to complete the round

    private void Start()
    {
        StartNewRound();
    }

    private void StartNewRound()
    {
        enemiesToSpawn = initialEnemiesPerRound + currentRound * 2; // Increase enemies per round
        enemiesRemaining = enemiesToSpawn;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn > 0)
        {
            // Spawn enemies based on current round, gradually introducing stronger types
            int maxEnemyIndex = Mathf.Min(currentRound / 3, enemyPrefabs.Count - 1); // Limits tier based on round
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, maxEnemyIndex + 1)];

            SpawnEnemy(enemyPrefab);
            enemiesToSpawn--;

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        // Find the player GameObject
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        // Generate a random position within the spawn radius around the player
        Vector2 randomPos2D = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.transform.position + new Vector3(randomPos2D.x, 0, randomPos2D.y);

        // Instantiate the enemy and assign the GameController reference to it
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Master_Enemy>().gameController = this; // Pass GameController reference to enemy
    }

    // This method should be called by enemies when they are destroyed
    public void OnEnemyDestroyed()
    {
        enemiesRemaining--;

        // If all enemies for the round are defeated, start the next round
        if (enemiesRemaining <= 0)
        {
            currentRound++;
            StartNewRound();
        }
    }
}
