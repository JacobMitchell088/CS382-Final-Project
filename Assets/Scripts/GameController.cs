using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this to use TextMeshPro

public class GameController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of different enemy prefabs (tiers)
    public List<GameObject> bossPrefabs; // List of special boss enemy prefabs
    public float bossHealthMultiplier = 1.0f; // Initial multiplier for boss health
    public float bossHealthScaleIncrease = 0.2f; // Increment for health scaling each boss wave
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
    private int bossRotation = 0;

    private void Start()
    {
        // Start the game with an initial delay of 5 seconds
        gameMusic = GetComponent<AudioSource>();
        StartCoroutine(StartWithDelay(5f));
        bossRotation = 0;
    }

    private void Update()
    {
        if (!gameMusic.isPlaying) { gameMusic.Play(); }
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
        // Check if it's a boss wave
        if (currentRound % 5 == 0 && bossPrefabs.Count > 0)
        {
            // Spawn a boss enemy
            GameObject bossPrefab = bossPrefabs[bossRotation];
            GameObject boss = SpawnEnemy(bossPrefab);

            Master_Enemy bossEnemy = boss.GetComponent<Master_Enemy>();
            if (bossEnemy != null)
            {
                bossEnemy.maxHealth = Mathf.RoundToInt(bossEnemy.maxHealth * bossHealthMultiplier);
                bossEnemy.currentHealth = bossEnemy.maxHealth; // Update current health to match max
            }

            bossHealthMultiplier += bossHealthScaleIncrease;

            enemiesToSpawn--; // Decrease count to reflect boss spawn
            enemiesRemaining--; // A single boss counts toward enemies to defeat

            bossRotation++;
            if(bossRotation > 2) {
                bossRotation = 0;
            }
        }

        // Spawn regular enemies
        while (enemiesToSpawn > 0)
        {
            int enemiesThisBatch = Mathf.Min(enemiesPerBatch, enemiesToSpawn);
            for (int i = 0; i < enemiesThisBatch; i++)
            {
                int maxEnemyIndex = Mathf.Min(currentRound / 3, enemyPrefabs.Count - 1);
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, maxEnemyIndex + 1)];
                SpawnEnemy(enemyPrefab);
                enemiesToSpawn--;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return null;
        }

        float angle = Random.Range(0f, 2 * Mathf.PI);
        Vector3 spawnPosition = player.transform.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Master_Enemy>().gameController = this; // Pass GameController reference to enemy

        return enemy; // Return the spawned enemy
    }


    public void OnEnemyDestroyed()
    {
        enemiesRemaining--;

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
