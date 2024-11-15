using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expValue = 10;
    public float rotationSpeed = 50f;

    void Update()
    {
        Vector3 rotationAxis = new Vector3(1, 0, 1).normalized;
        
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
