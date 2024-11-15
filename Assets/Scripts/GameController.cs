using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this to use TextMeshPro

public class GameController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of different enemy prefabs (tiers)
    public float spawnRadius = 10f; // Radius around the player for spawning enemies
    public int initialEnemiesPerRound = 10; // Initial number of enemies to spawn in the first round
    public float spawnInterval = 5f; // Initial time interval between enemy spawns
    public float spawnIntervalDecrease = 0.5f; // Amount to decrease spawn interval every 5 waves
    public float minimumSpawnInterval = 1f; // Minimum limit for spawn interval to avoid too fast spawning
    public int enemiesPerBatch = 5; // Number of enemies to spawn at once in each batch
    public TextMeshProUGUI roundText; // Reference to the TextMeshProUGUI component for displaying the round
    public AudioSource gameMusic;

    private int currentRound = 1;
    private int enemiesToSpawn;
    private int enemiesRemaining; // Enemies left to be defeated to complete the round

    private void Start()
    {
        // Start the game with an initial delay of 5 seconds
        StartCoroutine(StartWithDelay(5f));
    }

    private void Update() {
        if (!gameMusic.isPlaying) {gameMusic.Play();}
    }

    private IEnumerator StartWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewRound();
    }

    private void StartNewRound()
    {
        // Increase the number of enemies to spawn each round
        enemiesToSpawn = initialEnemiesPerRound + currentRound * 2;
        enemiesRemaining = enemiesToSpawn;

        // Update the round number on the screen
        UpdateRoundText();

        // Every 5 rounds, reduce the spawn interval to increase difficulty
        if (currentRound % 5 == 0 && spawnInterval > minimumSpawnInterval)
        {
            spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecrease, minimumSpawnInterval);
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn > 0)
        {
            // Spawn a batch of enemies
            int enemiesThisBatch = Mathf.Min(enemiesPerBatch, enemiesToSpawn);
            for (int i = 0; i < enemiesThisBatch; i++)
            {
                // Choose an enemy type based on current round, gradually introducing stronger types
                int maxEnemyIndex = Mathf.Min(currentRound / 3, enemyPrefabs.Count - 1);
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, maxEnemyIndex + 1)];

                SpawnEnemy(enemyPrefab);
                enemiesToSpawn--;
            }

            // Wait for the adjusted spawn interval before spawning the next batch
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

        // Spawn at a random point on the perimeter of the spawn radius
        float angle = Random.Range(0f, 2 * Mathf.PI);
        Vector3 spawnPosition = player.transform.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);

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

    private void UpdateRoundText()
    {
        if (roundText != null)
        {
            roundText.text = $"Round: {currentRound}";
        }
        else
        {
            Debug.LogWarning("Round TextMeshProUGUI reference is missing!");
        }
    }
}
