using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBullet : MonoBehaviour
{
    public int damage = 35;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has the Enemy script
        Master_Enemy enemy = other.GetComponent<Master_Enemy>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            //enemy.TakeDamage(damage);
        }
    }
}
