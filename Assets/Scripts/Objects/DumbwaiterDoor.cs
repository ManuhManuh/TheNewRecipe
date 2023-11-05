using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DumbwaiterDoor : ControlledObject
{
    public bool Frozen => frozen;

    [SerializeField] private float maxDistanceDeltaOpen;
    [SerializeField] private float maxDistanceDeltaClose;
    [SerializeField] private float minOpeningSizeForEntry;
    [SerializeField] private float doorCloseDelay;

    [SerializeField] private Transform openedPositionTransform;
    [SerializeField] private GameObject ladderPlaceholder;
    [SerializeField] private Rigidbody doorRigidbody;

    private Vector3 closedPosition;
    private Vector3 openedPosition;
    private Vector3 fromPosition;
    private Vector3 toPosition;
    public bool opening;
    public bool closing;
    public bool frozen;
    public bool delaying;
    private DumbwaiterPuzzle dumbwaiterPuzzle;

    private void Start()
    {
        closedPosition = transform.position;
        openedPosition = openedPositionTransform.position;
        opening = false;
        closing = false;
        frozen = false;
        delaying = false;

        dumbwaiterPuzzle = FindObjectOfType<DumbwaiterPuzzle>();
      
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
        if (!frozen && !closing && !delaying)
        {
            delaying = true;
            StartCoroutine(CloseDumbwaiterDoor(doorCloseDelay));

        }
       
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
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, maxDistanceDeltaClose * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        if (closing && collision.collider.CompareTag("DoorStopper")) // if the ladder is what the door collided with
        {
            Debug.Log("Door should be stopping");
            // Stop the door closing
            closing = false;

            // Find out how far open the door is
            var openAmount = Vector3.Distance(closedPosition, transform.position);

            // If the door is open enough to walk through
            if (openAmount > minOpeningSizeForEntry)
            {
                // Freeze the door open so it doesn't keep trying to close
                frozen = true;

                dumbwaiterPuzzle.CheckPuzzleStatus();
            }
            else
            {
                frozen = false;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
            // If the door has not finished closing yet and is not frozen, continue closing
            if (fromPosition != toPosition && !frozen)
            {
                closing = true;
            }
        }

    private IEnumerator CloseDumbwaiterDoor(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Set flags and positions for closing
        opening = false;
        closing = true;
        fromPosition = openedPosition;
        toPosition = closedPosition;

        // Play opening sound
        SoundManager.PlaySound(gameObject, "DumbwaiterDoorClose");

        delaying = false;
    }
}
