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
            opp.transform.position = Vector3.MoveTowards(item.transform.position, opp.walkPosition, opp.moveSpeed * Time.deltaTime);
            if(item.transform.position.y < 0 || item.transform.position.y > 0)
            {
                opp.transform.position = new Vector3(opp.transform.position.x, 0, opp.transform.position.z);
            }
        }
    }
}
