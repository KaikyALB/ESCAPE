using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    private float wallDistance;
    public float distanceLimit;

    public Transform predatorTransform;
    public GameObject wall;
    private void Start()
    {
        wall = GameObject.FindWithTag("Wall");
    }
    private void Update()
    {
        Vector3 wallPosition = wall.transform.position;
        Vector3 predatorPosition = predatorTransform.position;

        wallDistance = Vector3.Distance(predatorPosition, wall.transform.position);

        if(wallDistance < distanceLimit)
        {
            predatorTransform.position = new Vector3(predatorPosition.x - distanceLimit, 
                                            predatorPosition.y, 
                                            predatorPosition.z);
        }
    }
}
