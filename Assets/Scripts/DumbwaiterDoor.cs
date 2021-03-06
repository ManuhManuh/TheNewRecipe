using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbwaiterDoor : ControlledObject
{
    public float maxDistanceDeltaOpen;
    public float maxDistanceDeltaClose;
    public float minOpeningSizeForEntry;
    public Transform openedPositionTransform;
    public GameObject ladderPlaceholder;
    public GameObject interiorTeleportTarget;

    private Vector3 closedPosition;
    private Vector3 openedPosition;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    private Vector3 ladderRestPosition;
    private Quaternion ladderRestRotation;
    private bool opening;
    private bool closing;
    private bool frozen;
    

    private void Start()
    {
        closedPosition = transform.position;
        openedPosition = openedPositionTransform.position;
        opening = false;
        closing = false;
        frozen = false;

        // Find resting position for ladder when being propped and delete placeholder
        ladderRestPosition = ladderPlaceholder.transform.position;
        ladderRestRotation = ladderPlaceholder.transform.rotation;
        Destroy(ladderPlaceholder);
    }
    public override void OnPressed()
    {
        if (!frozen && !opening)
        {
            // Set flags and positions for opening
            opening = true;
            closing = false;
            fromPosition = closedPosition;
            toPosition = openedPosition;

            // Play opening sound
            SoundManager.PlaySound(gameObject, "DumbwaiterDoorOpen");
        }
        
    }

    public override void OnReleased()
    {
        if (!frozen && !closing)
        {
            // Set flags and positions for closing
            opening = false;
            closing = true;
            fromPosition = openedPosition;
            toPosition = closedPosition;

            // Play opening sound
            SoundManager.PlaySound(gameObject, "DumbwaiterDoorClose");
        }
       
    }
    private void FixedUpdate()
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
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, maxDistanceDeltaClose * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the door is not frozen and the ladder is what collided with it
        if (!frozen && other.CompareTag("DoorStopper"))
        {
            // Stop the door closing
            closing = false;

            // Find out how far open the door is
            var openAmount = Vector3.Distance(closedPosition, transform.position);

            // If the door is open enough to walk through
            if (openAmount > minOpeningSizeForEntry)
            {
                // Place the ladder nicely
                placeLadder(other.transform);

                // Freeze the door open so it doesn't keep trying to close
                frozen = true;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // If the door has not finished closing yet and is not frozen, continue closing
        if (fromPosition != toPosition && !frozen)
        {
            closing = true;
        }
    }

    private void placeLadder(Transform ladder)
    {
        // Move the ladder to the stable resting position
        ladder.position = ladderRestPosition;
        ladder.rotation = ladderRestRotation;

        // Disable the ladder from being picked up again
        ladder.GetComponent<GrabbableObject>().ObjectLocked = true;

        // Enable the teleport target inside the dumbwaiter
        interiorTeleportTarget.SetActive(true);
        Debug.Log("Teleport should now be enabled");
    }
}
