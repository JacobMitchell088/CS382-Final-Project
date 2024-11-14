using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalWeapon : MonoBehaviour
{
    public Transform player;             // Reference to the player's Transform
    public GameObject weaponPrefab;      // Prefab of the orbiting weapon
    public int numberOfWeapons = 3;      // Number of weapons to spawn
    public float orbitRadius = 2f;       // Distance of each weapon from the center
    public float orbitSpeed = 50f;       // Speed of the orbit
    public float yHeightOffset = 1;      // Y Height offset

    private void Update()
    {
        // Rotate the OrbitCenter around the player's position
        if (player != null)
        {
            Vector3 newPos = player.position;
            newPos.y = newPos.y - yHeightOffset;

            transform.position = newPos;
            transform.Rotate(Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }

    private void Start()
    {
        SpawnWeaponsAroundPlayer();
    }

    private void SpawnWeaponsAroundPlayer() {
        // Spawn a set number of weapons in a circular formation
        for (int i = 0; i < numberOfWeapons; i++)
        {
            // Calculate the angle for each weapon
            float angle = i * Mathf.PI * 2 / numberOfWeapons;
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;

            // Instantiate the weapon prefab and set it as a child of the OrbitCenter
            GameObject weapon = Instantiate(weaponPrefab, transform.position + spawnPosition, Quaternion.identity, transform);
            //weapon.transform.LookAt(player);
        }
    }
}
