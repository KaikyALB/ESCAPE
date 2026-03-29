using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public Player player;
    public Opponent opp;
    public GameObject ghostPrefab;

    public float ghostAmount = 0;
    private void Update()
    {
        if (opp.Spotted)
        {
            Invoke("ghostSpawn", 0.5f);
            if(ghostAmount >= 1)
            {
                CancelInvoke("ghostSpawn");
            }
        }
    }

    void ghostSpawn()
    {
        Instantiate(ghostPrefab, opp.storedPosition, transform.rotation);
        ghostAmount++;
    }
}
