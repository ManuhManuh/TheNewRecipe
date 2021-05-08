using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDrawer : GrabbableObject
{
    protected bool locked;
    private FixedJoint fixedJoint;
    private bool joined;

    // Locked is set in child scripts for drawers with different unlock conditions
    public override void OnGrab(ObjectGrabber grabber)
    {
        if (!locked)
        {
            // Create a fixed joint between this object and the grabber
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = grabber.GetComponent<Rigidbody>();
            joined = true;
        }
    }

    public override void OnDrop()
    {
        if (joined)
        {
            // Destroy the fixed joint between this object and the grabber
            Destroy(fixedJoint);
            joined = false;
        }
    }
}
