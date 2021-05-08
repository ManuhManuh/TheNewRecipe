using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : GrabWithJoint
{
    // public ConfigurableJoint joint;
    public float proximityToOrigin;
    public float maxDistanceDelta;

    protected Vector3 targetPosition;
    private bool dropped;
    private bool nearby;

    public override void Start()
    {
        base.Start();
        // Set the target position as the original position of the object
        targetPosition = transform.position;
        dropped = false;
    }

    public override void OnDrop()
    {
        base.OnDrop();

        // Check to see if the object is close enough to its original starting point (on all axes) to be snapped back there

        nearby = Vector3.Distance(targetPosition, transform.position) < proximityToOrigin;
        dropped = true;

    }

    private void Update()
    {

        if (dropped && nearby)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistanceDelta * Time.deltaTime);
        }
        
    }
}
