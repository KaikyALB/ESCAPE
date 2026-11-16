using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public Player player;
    public Opponent opp;
    public GameObject ghostPrefab;

    public float ghostAmount = 0;

    public float ghostTimer = 0;
    public float ghostDuration = 0.8f;

    public bool ghostActive = false;
    private void Start()
    {
        ghostTimer = ghostDuration;
    }
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
        if(ghostActive)
        {
            ghostTimer -= Time.deltaTime;
        }
        else if(!ghostActive)
        {
            ghostTimer = ghostDuration;
        }
        if(ghostTimer <= 0)
        {
            Destroy(ghostPrefab);

            ghostActive = false;
        }
    }

    void ghostSpawn()
    {
        Instantiate(ghostPrefab, opp.storedPosition, transform.rotation);
        ghostAmount++;
    }
}
