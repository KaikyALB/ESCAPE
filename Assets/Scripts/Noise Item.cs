using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoiseItem : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider col;

    public float breakF;
    public float radiusLimit;
    public float radiusExpand;
    private void Update()
    {
        if (col.enabled)
        {
            col.radius = col.radius + radiusExpand;
        }
        if (col.radius <= radiusLimit)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        print(collisionSpeed);

        col = GetComponentInChildren<SphereCollider>();

        if (collisionSpeed > breakF)
        {
            col.enabled = true;
        }
    }
}
