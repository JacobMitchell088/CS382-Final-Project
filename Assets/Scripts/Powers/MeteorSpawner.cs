using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;     // Prefab for the meteor
    public float spawnInterval = 3f;    // Time between each meteor spawn

    private void Start()
    {
        StartCoroutine(SpawnMeteors());
    }

    private IEnumerator SpawnMeteors()
    {
        while (true)
        {
            // Find a random enemy to target
            GameObject targetEnemy = FindRandomEnemy();

            if (targetEnemy != null)
            {
                // Spawn a meteor above the target enemy
                Vector3 spawnPosition = targetEnemy.transform.position + new Vector3(0, 30f, 0);
                Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            }

            // Wait for the interval before spawning the next meteor
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private GameObject FindRandomEnemy()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Return a random enemy if there are any
        if (enemies.Length > 0)
        {
            int randomIndex = Random.Range(0, enemies.Length);
            return enemies[randomIndex];
        }

        return null;
    }

    public void Upgrade()
    {
        spawnInterval = spawnInterval / 2;
        Debug.Log("Meteor Upgraded");
    }
}
