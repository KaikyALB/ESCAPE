using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private CapsuleCollider collider;
    public float startSpeed;
    public float moveSpeed;
    public float maxSpeed;
    public float crouchHeight = -0.2f;
    public float standingHeight;
    public bool isSprinting;

    public float crouchToStandTimer;
    public bool standReady;

    public bool isCrouching;
    public bool crouched;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        moveSpeed = startSpeed;
        standingHeight = collider.height;
    }
    void Update()
    {
        Movement();
    }
    public void Movement()
    {
        //Sets the movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //Speed to move and Space.World stops the rotation of the player from affecting the world
        transform.Translate(movement * moveSpeed * Time.deltaTime);
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
        if(Input.GetKey(KeyCode.LeftControl) && !crouched)
        {
            isCrouching = true;
        }
        if(isCrouching)
        {
            anim.SetBool("isCrouching", true);
            collider.height = isCrouching ? crouchHeight : standingHeight;
            crouched = true;
        }
        if(Input.GetKey(KeyCode.Tab))
        {
            anim.SetBool("isCrouching", false);
            collider.height = standingHeight;
            crouched = false;
            isCrouching = false;
            standReady = true;
        }
        if(!crouched & standReady)
        {
            anim.SetBool("crouchToStand", true);
            crouchToStandTimer -= Time.deltaTime;
            if(crouchToStandTimer < 0)
            {
                anim.SetBool("crouchToStand", false);
                standReady = false;
            }
        }
    }
}
