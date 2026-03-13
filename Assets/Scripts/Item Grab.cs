using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    Collider armCol;

    public GameObject grabText;
    public GameObject heldObject;
    public Transform attachPoint;

    public bool grabAvailable;
    public bool throwAvailable;
    void Start()
    {
        armCol = GetComponent<Collider>();
    }
    void Update()
    {
        if (heldObject != null && Input.GetMouseButton(0))
        {
            ThrowItem();
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
        heldObject = null;
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
