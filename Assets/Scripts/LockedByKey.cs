using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByKey : MonoBehaviour
{
    private bool locked;
    private bool available => !locked;
    private Vector3 keyPosition;
    private Quaternion keyInitialRotation;
    private Quaternion keyFinalRotation;
    private bool keyTurning;

    private void Start()
    {
        locked = true;
        gameObject.GetComponent<GrabbableObject>().OnAvailabilityChanged(available);

        // Find the sample key among child objects
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Key"))
            {
                // Record the position and rotation of the sample object
                keyPosition = child.position;
                keyInitialRotation = child.rotation;

                // Destroy the sample object
                Destroy(child.gameObject);
            }
        }
        keyTurning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked)
        {
            if (other.transform.CompareTag("Key"))
            {
                // Parent the key to the drawer
                other.transform.SetParent(gameObject.transform);

                // Move the key to the correction position and starting rotation
                other.transform.position = keyPosition;
                other.transform.rotation = keyInitialRotation;

                // TODO: Lerp to the correct rotation (turning key to keyFinalRotation)
                keyTurning = true;

                // Drop the key, using overload of OnDrop in GrabbableOjbect
                bool used = true;
                other.GetComponent<GrabbableObject>().OnDrop(used);

                // Make the key unavailable to be grabbed again
                bool keyAvailable = false;
                other.GetComponent<GrabbableObject>().OnAvailabilityChanged(keyAvailable);

                // unlock the drawer
                locked = false;
                gameObject.GetComponent<GrabbableObject>().OnAvailabilityChanged(available);
            }
        }
    }

    private void LateUpdate()
    {
        if (keyTurning)
        {
            // TODO: Here's where the lerp goes
        }
    }
}   
