using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByKey : LockableObject
{
    private Animator keyAnimator;

    private Vector3 keyPosition;
    private Quaternion keyRotation;
    private Transform key;

    protected override void Start()
    {
        // Find the sample key among child objects
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Key"))
            {
                // Record the position and rotation of the sample key
                keyPosition = child.position;
                keyRotation = child.localRotation;

                // Destroy the sample object
                Destroy(child.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check to see if the drawer is locked
        if (locked)
        {
            // Check that a key is what triggered
            if (other.transform.CompareTag("Key"))
            {
                key = other.transform;

                // Get the key's animator
                keyAnimator = key.GetComponent<Animator>();

                // Tell the key that it has been used
                // key.GetComponent<GrabbableObject>().ObjectUsed = true;

                // Parent the key to the drawer
                key.SetParent(gameObject.transform);

                // Play the key insert sound
                SoundManager.PlaySound(gameObject, "KeyInsert");

                // Drop the key
                // key.GetComponent<GrabbableObject>().OnDrop();

                // Set the key to its starting position
                key.position = keyPosition;
                key.localRotation = keyRotation;

                // Play the key turning sound
                SoundManager.PlaySound(gameObject, "KeyTurn");
                
                // Play the key turning animation
                if(keyAnimator != null)
                {
                    keyAnimator.SetBool("KeyTurning", true);
                }

                // Unlock the drawer
                base.OnUnlocked();
            }
        }
    }
}   
