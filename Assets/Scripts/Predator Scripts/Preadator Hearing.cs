using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreadatorHearing : MonoBehaviour
{
    public Opponent opp;
    public NoiseItem item;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Noise Object")
        {
            opp.transform.position = Vector3.MoveTowards(item.transform.position, item.transform.position, item.transform.position.z);
        }
    }
}
