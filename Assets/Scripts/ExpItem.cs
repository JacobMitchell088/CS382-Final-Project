using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expValue = 10;
    public float rotationSpeed = 50f;

    public float attractionRadius = 2.5f;
    public float attractionSpeed = 20f;
    private Transform playerTransform;

    void Update()
    {
        Vector3 rotationAxis = new Vector3(1, 0, 1).normalized;
        
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);


            if (distanceToPlayer <= attractionRadius)
            {
                // Move towards player
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * attractionSpeed * Time.deltaTime;
            }
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        Destroy(gameObject, 30f);
    }
}
