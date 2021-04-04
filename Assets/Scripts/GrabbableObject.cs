using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    //public GameObject gripTarget;

    private Rigidbody rigidBody;

    public virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void OnGrab(ObjectGrabber grabber)
    {
        // Child this object to the grabber
        transform.SetParent(grabber.transform);

        // Turn off physics
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }

    public virtual void OnDrop()
    {

         // Unparent the object
        transform.SetParent(null);

        // Turn on physics
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false; 

    }

    public virtual void OnTriggerStart()
    {
    }

    public virtual void OnTriggerEnd()
    {
    }
}
