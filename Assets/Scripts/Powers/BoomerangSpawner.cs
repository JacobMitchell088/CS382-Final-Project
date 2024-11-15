using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpawner : MonoBehaviour
{
    public GameObject boomerangBulletPrefab;   // Prefab for the bullet
    public Transform player;                   // Reference to the player
    public int boomerangCount = 3;             // Number of active bullets
    public float spawnInterval = 2f;           // Time between bullet spawns

    private float timer = 0f;                 // Timer to manage spawn intervals

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

    private void SpawnBullet() {
       Instantiate(boomerangBulletPrefab, player.position, player.rotation);
    }

    public void Upgrade()
    {
        // TODO -- Upgrade weapon
        spawnInterval = spawnInterval / 2;
        //Debug.Log("Boomerang Upgraded");
    }
}
