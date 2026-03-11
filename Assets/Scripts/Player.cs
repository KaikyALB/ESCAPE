using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Opponent opp;
    Transform cam;

    public enum sanity {Afraid, Anxious, Desperate, Chaotic}

    private Rigidbody rb;
    private Animator anim;
    public CapsuleCollider col;
    public float startSpeed;
    public float moveSpeed;
    public float crouchSpeed;
    public float maxSpeed;
    public float crouchHeight = 0.39f;
    public float standingHeight;
    public bool isSprinting;

    public float crouchToStandTimer;
    public bool standReady;

    public bool isCrouching;
    public bool crouched;

    public float mouseSense = 5f;
    private float lookRotation = 0f;
    public List<Vector3> lastPosition = new List<Vector3>();
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        cam = Camera.main.transform;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        moveSpeed = startSpeed;
        standingHeight = 0.78f;
    }
    void Update()
    {
        Movement();

        lastPosition.Add(transform.position);//Stores the last position of the game object

        if (lastPosition.Count > 30)//If list bigger than 30, clears
        {
            lastPosition.Clear();
        }
        MouseLook();
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
            crouched = true;
            moveSpeed = crouchSpeed;
            col.height = crouchHeight;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            anim.SetBool("isCrouching", false);
            col.radius = standingHeight;
            crouched = false;
            isCrouching = false;
            moveSpeed = startSpeed;
        }
        /*if(!crouched & standReady)
        {
            anim.SetBool("crouchToStand", true);
            crouchToStandTimer -= Time.deltaTime;
            if(crouchToStandTimer < 0)
            {
                anim.SetBool("crouchToStand", false);
                standReady = false;
            }
        }*/
    }
    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense;

        lookRotation -= mouseY;
        lookRotation = Mathf.Clamp(lookRotation, -85, 85);

        cam.localRotation = Quaternion.Euler(lookRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
