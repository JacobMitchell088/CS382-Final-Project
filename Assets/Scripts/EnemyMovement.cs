using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public float speed = 3.5f; // Public variable to set speed

    private NavMeshAgent agent;
    private Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void Start()
    {
        // Find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}