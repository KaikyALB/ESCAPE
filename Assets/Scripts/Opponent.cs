using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Player player;
    public enum state {Unaware, Warned, Spotted};
    
    private Collider viewCollider;
    private Animator anim;
    private Rigidbody rb;
    public float moveSpeed;
    public float sprintSpeedMultiplier;

    public bool Spotted;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        viewCollider = GetComponentInChildren<Collider>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {

        if (Spotted)
        {
            Warned();
            anim.SetBool("isWalking", true);
        }
        print(rb.velocity.magnitude);
    }
    
    public void Unaware()
    {

    }
    public void Warned()
    {
        print("Warned State");
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);//Moves towards the target
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spotted = true;
        }
    }
}
