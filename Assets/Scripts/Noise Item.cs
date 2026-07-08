using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoiseItem : MonoBehaviour
{
    private Rigidbody rb;
    public SphereCollider col;

    public float breakF;
    public float radiusLimit;
    public float radiusExpand;
    private void Start()
    {
        col.enabled = false;
    }
    private void Update()
    {
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
