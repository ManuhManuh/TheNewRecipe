using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByWinePuzzle : ObjectReturn
{
    public GameObject handle;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetComponent<GrabbableObject>().ObjectLocked = true;
    }

    public override void OnGrab(ObjectGrabber grabber)
    {
   
        // If the drawer has been checked and found unlocked already
        if (GetComponent<GrabbableObject>().ObjectLocked == false)
        {
            base.OnGrab(grabber);
        }
        else
        {
            // Check if the drawer was unlocked since last attempt
            if (GameManager.wineBottlesPlaced)
            {
                // Unlock the drawer
                GetComponent<GrabbableObject>().ObjectLocked = false;

                // Grab the drawer and open it
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
