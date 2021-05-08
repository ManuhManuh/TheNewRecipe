using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottle : GrabbableObject
{
    public bool moveWithJoint;
    public float proximityToOrigin;
    public float maxDistanceDelta;
    public float proximityToTarget;
    public bool InAWineSlot => inAWineSlot;
    public bool InACorrectSlot => inACorrectSlot;

    private bool dropped;
    private bool inAWineSlot;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool movingToSlot;
    private bool inACorrectSlot;
    private GameObject baseWineRack;

    public override void Start()
    {
        base.Start();

        // Initialize statuses
        dropped = false;
        inAWineSlot = false;
        baseWineRack = GameObject.Find("Base Wine Rack");
    }

    public override void OnGrab(ObjectGrabber grabber)
    {
        // Reset everything to do with moving to a wine slot
        dropped = false;
        movingToSlot = false;
        targetPosition = Vector3.zero;
        targetRotation = Quaternion.identity;

        // Do the normal grabbing activities
        base.OnGrab(grabber);
    }

    public override void OnDrop()
    {
        Debug.Log($"{gameObject.name} dropped while InAWineSlot was {inAWineSlot}");
        // If we have triggered a slot
        if (inAWineSlot)
        {
            // Unparent the bottle from the hand, and reparent it to the wine rack (but don't restore physics to it)
            gameObject.transform.SetParent(baseWineRack.transform);

            // Flag the bottle as moving to the slot so the update method will do the moving
            movingToSlot = true;

            // Snap for testing - move to update if this works
            transform.rotation = targetRotation;
            transform.position = targetPosition;

            movingToSlot = false;
        }
        // If we have not encountered a slot
        else
        {
            // Call the base method, as the object should drop normally with physics turned on
            base.OnDrop();
        }
    }

    public void OnSlotEntered(Vector3 homePosition, Quaternion homeRotation, bool correctSlot)
    {
        Debug.Log($"{gameObject.name} entered a slot");
        targetPosition = homePosition;
        targetRotation = homeRotation;

        // Flag as being in a slot, and whether or not it's a correct one
        inAWineSlot = true;
        inACorrectSlot = correctSlot;

        // Register the insertion with the Game Manager
        GameManager.OnWineBottlePlaced();
    }

    public void OnSlotExited()
    {
        Debug.Log($"{gameObject.name} exited a slot");

        // Flag as not being in a slot any more
        inAWineSlot = false;
        inACorrectSlot = false;

        // Register removal with Game Manager
        GameManager.OnWineBottleRemoved();
    }

}
