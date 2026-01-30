using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private Collider viewCollider;

    public bool Spotted;
    void Start()
    {
        viewCollider = GetComponentInChildren<Collider>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spotted = true;
        }
    }
}
