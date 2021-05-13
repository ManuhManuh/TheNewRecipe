using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByWinePuzzle : GrabWithJoint
{
    public bool locked;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnGrab(ObjectGrabber grabber)
    {
        // If the drawer has been checked and found unlocked already
        if (!locked)
        {
            base.OnGrab(grabber);
        }
        else
        {
            // Check if the drawer was unlocked since last attempt
            if (GameManager.wineBottlesPlaced)
            {
                locked = false;
                base.OnGrab(grabber);
            }
            else
            {
                // TODO: Make this an audio comment
                Debug.Log("Locked!");
            }
            
        }

    }

}
