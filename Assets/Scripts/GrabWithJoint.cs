﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWithJoint : GrabbableObject
{

    private FixedJoint fixedJoint;

    public override void OnGrab(ObjectGrabber grabber)
    {
        // Create a fixed joint between this object and the grabber
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = grabber.GetComponent<Rigidbody>();

        //Debug.Log ($"Created a fixed joint between {gameObject.name} and {grabber.name}");
    }

    public override void OnDrop()
    {
        // Destroy the fixed joint between this object and the grabber

        Destroy(fixedJoint);

    }

}
