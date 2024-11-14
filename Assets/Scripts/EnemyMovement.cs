using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public float speed = 3.5f; // Speed of the enemy
    public float rotationSpeed = 5f; // Rotation speed for smooth turning
    public float attackRange = 2f; // Attack range
    public float attackDuration = 1f; // Duration of the attack animation (in seconds)

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private bool isDead = false; // To track if the enemy is dead
    private bool isAttacking = false; // To prevent multiple attack triggers

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
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
        if (isDead) return; // Don't update movement or animations if the enemy is dead

        if (player != null)
        {
            // Calculate distance to player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // If the enemy is within attack range, stop moving and attack
            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                Attack();
            }
            else if (distanceToPlayer > attackRange && !isAttacking)
            {
                // Set the destination to the player's position if not in attack range
                agent.SetDestination(player.position);
                animator.SetBool("IsWalking", true);
            }

            // Handle rotation towards the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep rotation in the horizontal plane

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Public function to trigger the attack animation
    public void Attack()
    {
        if (!isDead)
        {
            isAttacking = true; // Set attacking state to true to prevent multiple triggers
            animator.SetBool("IsAttacking", true);
            StartCoroutine(AttackCoroutine()); // Start the coroutine to stop the attack after the duration
        }
    }

    // Coroutine to stop the attack after a set duration
    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDuration); // Wait for the duration of the attack animation
        StopAttack(); // Stop the attack after the duration
    }

    // Public function to stop the attack animation
    public void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
        isAttacking = false; // Reset the attacking state once the animation is done
    }

    // Public function to trigger the death animation
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("IsDead", true);
            agent.isStopped = true; // Stop movement when dead
            Destroy(gameObject, 2f); // Destroy after 2 seconds, or manage it how you like
        }
    }
}
