using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityViewCount : MonoBehaviour
{
    public InsanitySystem IS;

    public float countLimit;
    public float countTimer;
    public float countCooldown;
    public float count;

    public bool onView;

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            onView = true;
        }
        countTimer = countCooldown;
    }
    private void OnTriggerStay(Collider other)
    {
        onView = true;
        if(other.tag == "Player" && onView)
        {
            count += Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        countTimer -= Time.deltaTime;
        if(countCooldown <= 0)
        {
            onView = false;
            count = 0;
        }
    }
}
