using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : GrabbableObject
{

    public override void OnDrop()
    {
        transform.SetParent(null);

        // do NOT turn on physics, as the object will be placed somewhere
    }
}
