using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private CapsuleCollider collider;
    public float startSpeed;
    public float moveSpeed;
    public float maxSpeed;
    public bool isSprinting;

    public bool isCrouching;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        moveSpeed = startSpeed;
    }
    void Update()
    {
        Movement();
        Vector3 Coordinate = transform.position;
    }
    public void Movement()
    {
        //Sets the movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //Speed to move and Space.World stops the rotation of the player from affecting the world
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        anim.SetBool("isWalking", movement.magnitude > 0.01f);
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
        if(isSprinting)
        {
            moveSpeed = maxSpeed;
            anim.SetBool("isSprinting", true);
        }
        else if(!isSprinting)
        {
            moveSpeed = startSpeed;
            anim.SetBool("isSprinting", false);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        if(isCrouching)
        {
            anim.SetBool("isCrouching", true);
        }
    }
}
