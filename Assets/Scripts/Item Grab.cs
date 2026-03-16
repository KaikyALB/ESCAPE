using System.Collections;
using System.Collections.Generic;
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
    private bool isInPickupRange = false;
    private Collider currentPickupCollider = null;

    void Start()
    {
        lr.GetComponentInParent<LineRenderer>();
        armCol = GetComponent<Collider>();
        if (lr != null)
        {
            lr.enabled = false;
        }
        if (playerCam == null)
        {
            playerCam = Camera.main;
        }
    }

    void Update()
    {
        if (heldObject != null && Input.GetMouseButton(1))
        {
            UpdateTrajectory();
        }
        else if (lr != null)
        {
            lr.enabled = false;
        }
        if (Input.GetMouseButton(0))
        {
            if (heldObject != null)
            {
                ThrowItem();
            }
        }
        if (isInPickupRange && Input.GetKey(KeyCode.E) && heldObject == null && currentPickupCollider != null)
        {
            GrabItem(currentPickupCollider);
        }
    }
    private void GrabItem(Collider other)
    {
        // Critical order: disable physics BEFORE parenting to prevent push/drift
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;         // clear any drift momentum
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = other.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Now parent & snap
        other.gameObject.transform.SetParent(attachPoint);
        other.transform.localPosition = Vector3.zero;
        other.transform.localRotation = Quaternion.identity;

        heldObject = other.gameObject;

        // Cleanup
        grabText.SetActive(false);
        grabAvailable = false;
        isInPickupRange = false;
        currentPickupCollider = null;

        Debug.Log("Grabbed: " + heldObject.name);
    }
    private void UpdateTrajectory()
    {
        if (heldObject == null || lr == null)
        {
            if (lr == null)
            {
                lr.enabled = false;
                return;
            }
        }
        lr.enabled = true;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        Vector3 startPos = attachPoint.position;
        Vector3 velocity = ray.direction * throwForce + Vector3.up * throwingArc;
        lr.positionCount = lrArc;
        Vector3 currentPos = startPos;
        Vector3 currentVel = velocity;
        for (int i = 0; i < lrArc; i++)
        {
            lr.SetPosition(i, currentPos);
            currentVel += Physics.gravity * lrTimeStep;
            currentPos += currentVel * lrTimeStep;
            if (Physics.Raycast(currentPos, currentVel.normalized, lrTimeStep * currentVel.magnitude + 0.1f))
            {
                lr.positionCount = i + 1;
                break;
            }
            if (Vector3.Distance(startPos, currentPos) > Maxlr)
            {
                lr.positionCount = i + 1;
                break;
            }
        }
    }

    void ThrowItem()
    {
        if (heldObject == null) return;
        heldObject.transform.SetParent(null);//Removes ther parent making the object able to move
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            Vector3 throwDirection = ray.direction;
            rb.AddForce(throwDirection * throwForce + Vector3.up * throwingArc, ForceMode.Impulse);//Throws the object
        }
        Collider col = heldObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
        heldObject = null;
    }

    void Aim()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise Object") && heldObject == null)
        {
            grabText.SetActive(true);
            grabAvailable = true;
            isInPickupRange = true;
            currentPickupCollider = other;  // remember which one
            Debug.Log("Entered range - hold E to grab");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Optional: keep prompt alive if needed, but no pickup logic here
        if (other.CompareTag("Noise Object") && heldObject == null)
        {
            grabText.SetActive(true);
            grabAvailable = true;
            isInPickupRange = true;
            currentPickupCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Noise Object"))
        {
            grabText.SetActive(false);
            grabAvailable = false;
            isInPickupRange = false;
            currentPickupCollider = null;
            Debug.Log("Exited range");
        }
    }
}