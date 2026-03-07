using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Player player;
    public enum state {Unaware, Warned, Pursue};
    
    private Collider viewCollider;
    private Animator anim;
    private Rigidbody rb;
    public float moveSpeed;
    public float sprintSpeedMultiplier;

    public int framesAgo = 10;
    public Vector3 storedPosition;
    private bool positionStored = false;
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
            if (!positionStored && player.lastPosition.Count > framesAgo)
            {
                storedPosition = player.lastPosition[player.lastPosition.Count - framesAgo];//This stores the latest position on the list
                positionStored = true;
            }
            Warned();
        }
        if(Vector3.Distance(transform.position, storedPosition) < 0.01f)
        {
            Spotted = false;
            positionStored = false;
        }
    }
    
    public void Unaware()
    {

    }
    public void Warned()
    {
        transform.position = Vector3.MoveTowards(transform.position, storedPosition, moveSpeed * Time.deltaTime);//Moves towards the target
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Spotted = true;
        }
    }
}
