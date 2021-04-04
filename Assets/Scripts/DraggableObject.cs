using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : GrabbableObject
{
    public float springForce;
    public float dampingForce;

    private SpringJoint joint;

    public override void OnGrab(ObjectGrabber grabber)
    {
        // Add a spring joint between this object's rigidbody and the grabber's rigidbody
        joint = gameObject.AddComponent<SpringJoint>();
        joint.spring = springForce;
        joint.damper = dampingForce;
        joint.connectedBody = grabber.GetComponent<Rigidbody>();

    }

    public override void OnDrop()
    {
        // Remove the spring joint
        Destroy(joint);

    }
}
