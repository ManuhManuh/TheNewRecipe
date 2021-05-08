using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbwaiterDoor : ControlledObject
{
    public float maxDistanceDeltaOpen;
    public float maxDistanceDeltaClose;
    public Transform openedPositionTransform;

    private Vector3 originalPosition;
    private Vector3 openedPosition;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private bool opening;
    private bool closing;

    private void Start()
    {
        originalPosition = transform.position;
        openedPosition = openedPositionTransform.position;
        opening = false;
        closing = false;
    }
    public override void OnPressed()
    {
        // Set flags and positions for opening
        opening = true;
        closing = false;
        fromPosition = originalPosition;
        toPosition = openedPosition;

    }

    public override void OnReleased()
    {
        // Set flags and positions for closing
        opening = false;
        closing = true;
        fromPosition = openedPosition;
        toPosition = originalPosition;

    }
    private void Update()
    {
        if (fromPosition == toPosition)
        {
            opening = false;
            closing = false;
        }

        if (opening)
        {
            // Open the door
            transform.position = Vector3.MoveTowards(transform.position, openedPosition, maxDistanceDeltaOpen * Time.deltaTime);
        }

        if (closing)
        {
            // Close the door
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, maxDistanceDeltaClose * Time.deltaTime);
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        // If something other than the player has collided with the door
        if (collision.collider.CompareTag("DoorStopper"))
        {
            closing = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        // If the door has not finished closing yet, continue closing
        if (fromPosition != toPosition)
        {
            closing = true;
        }
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        // If something other than the player has collided with the door
        // if (other.collider.CompareTag("DoorStopper"))
        //{
            closing = false;
       // }
    }
    private void OnTriggerExit(Collider other)
    {
        // If the door has not finished closing yet, continue closing
        if (fromPosition != toPosition)
        {
            closing = true;
        }
    }
}
