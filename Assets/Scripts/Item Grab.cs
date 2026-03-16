using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    Collider armCol;

    public Camera playerCam;
    public LineRenderer lr;

    public GameObject grabText;
    public GameObject heldObject;
    public Transform attachPoint;

    public bool grabAvailable;
    public bool throwAvailable;

    public float throwForce;
    public float throwingArc;

    public int lrArc = 40;
    public float lrTimeStep = 0.03f;
    public float Maxlr = 50f;
    void Start()
    {
        lr.GetComponentInParent<LineRenderer>();
        armCol = GetComponent<Collider>();
    }
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Aim();
        }
        if (heldObject != null && Input.GetMouseButton(0))
        {
            ThrowItem();
        }
        UpdateTrajectory();
    }
    private void UpdateTrajectory()
    {
        if(heldObject == null || lr == null)
        {
            if(lr == null)
            {
                lr.enabled = false;
                return;
            }
        }
        lr.enabled = true;

        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        Vector3 startPos = heldObject.transform.position;
        Vector3 velocity = ray.direction * throwForce + Vector3.up * throwingArc;

        lr.positionCount = lrArc;

        Vector3 currentPos = startPos;
        Vector3 currentVel = velocity;

        for(int i = 0; i < lrArc; i++)
        {
            lr.SetPosition(i, currentPos);

            currentVel += Physics.gravity * lrTimeStep;
            currentPos += currentVel * lrTimeStep;

            if(Physics.Raycast(currentPos, currentVel.normalized, lrTimeStep * currentVel.magnitude + 0.1f))
            {
                lr.positionCount = i + 1;
                break;
            }

            if(Vector3.Distance(startPos, currentPos) > Maxlr)
            {
                lr.positionCount = i + 1;
                break;
            }
        }
    }
    void ThrowItem()
    {
        heldObject.transform.SetParent(null);//Removes ther parent making the object able to move

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 50f + Vector3.up * 10f, ForceMode.Impulse);//Throws the object
        }
        Collider col = heldObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        Vector3 throwDirection = ray.direction;

        rb.AddForce(throwDirection * throwForce + Vector3.up * throwingArc, ForceMode.Impulse);

        heldObject = null;
    }
    void Aim()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Noise Object")
        {
            grabText.SetActive(true);
            grabAvailable = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Noise Object")
        {
            grabText.SetActive(true);
            grabAvailable = true;
        }
        if (grabAvailable && other.tag == "Noise Object" && Input.GetKey(KeyCode.E))
        {
            other.gameObject.transform.SetParent(attachPoint);
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;//Keeps the rotation
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = true; //Disables the physics
            }
            Collider col = other.GetComponent<Collider>();
            if(col != null)
            {
                col.enabled = false; //Disables the collider so it doesnt trigger anymore
            }
            heldObject = other.gameObject;//Sets the heldObject gameObject
        }
    }
    private void OnTriggerExit(Collider other)
    {
        grabText.SetActive(false);
        grabAvailable = false;
    }
}
