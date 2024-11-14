using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawner : MonoBehaviour
{
    public GameObject boomerangBulletPrefab;   // Prefab for the bullet
    public Transform player;                   // Reference to the player
    public int boomerangCount = 3;             // Number of active bullets
    public float spawnInterval = 2f;           // Time between bullet spawns

    private int currentActiveBullets = 0;      // Track active bullets
    private float timer = 0f;                  // Timer to manage spawn intervals

    private void Update()
    {
        // Update timer and check if we need to spawn another bullet
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBullet();
            timer = 0f; // Reset timer
        }
    }

    private void SpawnBullet()
    {
        

        // Instantiate a new boomerang bullet near the player
        GameObject bullet = Instantiate(boomerangBulletPrefab, player.position, player.rotation);

        // Optional: Assign player reference to the bullet (if needed)
        BoomerangBullet bulletScript = bullet.GetComponent<BoomerangBullet>();
        bulletScript.player = player;

        // Track the new bullet
        currentActiveBullets++;

        // Destroy the bullet after a certain time or when it returns, and decrement the counter
        StartCoroutine(HandleBulletLifetime(bullet));
    }

    private IEnumerator HandleBulletLifetime(GameObject bullet)
    {
        yield return new WaitForSeconds(2f); // lifetime
        if (bullet != null)
        {
            Destroy(bullet);
            currentActiveBullets--;
        }
    }
}
