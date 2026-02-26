using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public enum state {Unaware, Warned, Spotted};
    
    private Collider viewCollider;

    public float speed;

    public float walkTimer = 0f;
    public float walkDuration = 5f;

    public bool startWalk;
    public bool Spotted;
    void Start()
    {
        viewCollider = GetComponentInChildren<Collider>();
    }
    void Update()
    {
        switch(state.Unaware)
        {
            case state.Unaware:
                startWalk = true;
                Unaware();
                break;
            case state.Warned:
                Spotted = true;
                Warned();
                break;
        }
        if(startWalk)
        {
            walkTimer = walkDuration;
        }
            
    }
    public void Unaware()
    {
        float randomX = Random.Range(0, 200);
        Vector3 randomMoving = new Vector3(randomX * speed, 0, 0);//Moves the enemy to different directions
        transform.position += randomMoving;
        walkTimer -= Time.deltaTime;
        if(walkTimer < 0)
        {
            startWalk = false;
        }
    }
    public void Warned()
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
