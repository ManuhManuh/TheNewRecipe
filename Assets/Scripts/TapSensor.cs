using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSensor : MonoBehaviour
{
    public bool HasTap => hasTap;

    private Transform tap;
    private Vector3 tapRestPosition;
    private Quaternion tapRestRotation;
    private bool hasTap;

    private void Start()
    {
        // Find the sample key among child objects
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Tap"))
            {
                // Record the position and rotation of the sample object
                tapRestPosition = child.position;
                tapRestRotation = child.rotation;

                // Destroy the sample object
                Destroy(child.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that hit was a tap
        if (other.CompareTag("Tap"))
        {
            tap = other.transform;

            // Let the tap know it has been dropped and is unavailable to be grabbed
            tap.GetComponent<GrabbableObject>().ObjectUsed = true;

            // Drop the tap
            tap.GetComponent<GrabbableObject>().OnDrop();

            // Place the tap
            tap.position = tapRestPosition;
            tap.rotation = tapRestRotation;

            // Parent the tap to the sensor 
            tap.SetParent(this.transform);

            // update status of slot to filled
            hasTap = true;

            // Destroy the mesh collider to keep the jitters away ;)
            Destroy(gameObject.GetComponent<MeshCollider>());

            // Let the Game Manager know that the slot was filled
            GameManager.OnKegTapped();

        }

    }
}
