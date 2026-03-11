using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Player player;

    public TerrainData td;

    public enum state {Unaware, Warned, Pursue};

    public state currentState;

    private Collider viewCollider;
    private Animator anim;
    private Rigidbody rb;
    public float moveSpeed;
    public float sprintSpeedMultiplier;

    public float initialWalk = 6f;
    public float walkTime = 0f;

    Vector3 walkPosition;
    public bool walkReached;
    public bool isUnawared;
    public int framesAgo = 10;
    public Vector3 storedPosition;
    private bool positionStored = false;
    public bool Spotted;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        viewCollider = GetComponentInChildren<Collider>();
        anim = GetComponent<Animator>();

        currentState = state.Unaware;

        walkTime = initialWalk;
    }
    void Update()
    {
        if (!Spotted)
        {
            walkTime -= Time.deltaTime;
            Unaware();
            if (Vector3.Distance(transform.position, walkPosition) < 0.05f)
            {
                transform.position = walkPosition;
            }
        }
        switch (currentState)
        {
            case state.Unaware:
                InvokeRepeating("ChooseRandomPos", 0f, 15f);
                break;
            case state.Warned:
                Warned();
                break;
            case state.Pursue:
                Pursue();
                break;
        }
        if(isUnawared)
        {
            Unaware();
            if (Vector3.Distance(transform.position, walkPosition) < 0.01f)
            {
                walkReached = false;
            }
        }
        if (Spotted)
        {
            if (!positionStored && player.lastPosition.Count > framesAgo)
            {
                storedPosition = player.lastPosition[player.lastPosition.Count - framesAgo];//This stores the latest position on the list
                positionStored = true;
            }
            currentState = state.Warned;
        }
        if(Vector3.Distance(transform.position, storedPosition) < 0.01f)
        {
            Spotted = false;
            positionStored = false;
        }
    }
    
    public void Unaware()
    {
        if(walkTime <= 0) 
        {
        transform.position = Vector3.MoveTowards(transform.position, walkPosition, moveSpeed * Time.deltaTime);
        }
        walkTime = initialWalk;
    }
    public void ChooseRandomPos()
    {
        float randomDistanceX = Random.Range(0, td.size.x/4);
        float randomDistanceZ = Random.Range(0, td.size.y/4);
        Vector3 randomDistance = new Vector3(randomDistanceX, 0, randomDistanceZ);
        walkPosition = transform.position + randomDistance;
    }
    public void Warned()
    {
        transform.position = Vector3.MoveTowards(transform.position, storedPosition, moveSpeed * Time.deltaTime);//Moves towards the target
    }
    public void Pursue()
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
