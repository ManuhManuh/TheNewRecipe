using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    //public GameObject gripTarget;

    protected Rigidbody rigidBody;

    public virtual void Start()
    {
        
    }

    public virtual void OnGrab(ObjectGrabber grabber)
    {
        //Debug.Log($"{ this.name} was grabbed");
        gameObject.TryGetComponent<Rigidbody>(out rigidBody);

        // If there is no rigidbody on the item (because it was in a drawer for example)
        if (rigidBody == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rigidBody = GetComponent<Rigidbody>();
        }

        // Child this object to the grabber
        transform.SetParent(grabber.transform);
       
        // Turn off physics
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }

    public virtual void OnDrop()
    {
        //Debug.Log($"{ this.name} was dropped");
         // Unparent the object
        transform.SetParent(null);

        if (rigidBody != null)
        {
            // Turn on physics
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
        }

    }

    public virtual void OnTriggerEnd()
    {
    }
}
