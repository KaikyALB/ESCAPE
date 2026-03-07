using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public Player player;
    public Opponent opp;
    public GameObject ghostPrefab;


    private void Update()
    {
        if (opp.Spotted)
        {
            Invoke("ghostSpawn", 0.5f);
        }
    }

    void ghostSpawn()
    {
        Instantiate(ghostPrefab, opp.storedPosition, transform.rotation);
    }
}
