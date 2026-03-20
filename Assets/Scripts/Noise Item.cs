using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoiseItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    private Collider noiseCol;

    public float breakF;

    public Vector3 colliderExpand = new Vector3(15, 15, 15);
    private void Update()
    {
        noiseCol = GetComponentInChildren<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        print(collisionSpeed);

        if (collisionSpeed < breakF)
        {
            noiseCol.enabled = true;
            if(noiseCol.enabled)
            {
                noiseCol.transform.localScale += colliderExpand;
            }
        }
    }
}
