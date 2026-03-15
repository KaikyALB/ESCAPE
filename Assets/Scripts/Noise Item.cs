using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        print(collisionSpeed);
    }
}
