using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public string gripInputName;
    public string triggerInputName;

    private GrabbableObject grabbedObject;
    private TouchableObject touchedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the right contoller grip was pressed
        if(Input.GetButtonDown(gripInputName))
        {
            // Change the animation from idle to grip
            GetComponent<Animator>().SetBool("Gripped", true);

            // Check if we are touching a grabbable object
            if (grabbedObject != null)
            {

                // Pick up the object 
                grabbedObject.OnGrab(this);
            }
        }

        if (Input.GetButtonUp(gripInputName))
        {
            // Change the animation from grip to idle
            GetComponent<Animator>().SetBool("Gripped", false);

            // Check if we are holding any Grabbable object (may not be stored in grabbedObject due to timing mishaps)
            foreach (Transform child in transform)
            {
                if (child.GetComponent<GrabbableObject>() && grabbedObject == null)
                {
                    grabbedObject = child.GetComponent<GrabbableObject>();
                }
            }

            if(grabbedObject != null)
            {
                // Drop the object we are holding
                grabbedObject.OnDrop();

                // Forget the object that was held
                grabbedObject = null;
            }

        }

    }

    public void OnTriggerEnter(Collider other)
    {

        // Check if the object we touched is touchable (has the touchable script on it) - this just changes the colour of the object
        TouchableObject touchable = other.GetComponent<TouchableObject>();

        if (touchable != null)
        {
            // Let the object know it was touched
            touchable.OnTouched();

            // Store the currently touched object
            touchedObject = touchable;
        }

        // Check if the object we touched is grabbable (has the grabbable script on it)
        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();

        // If we aren't already holding something
        if (grabbable != null)
        {
            // Check if the object is available to be grabbed
            if (!grabbable.ObjectUsed && !grabbable.ObjectLocked)
            {
                // Store the current grabbable object
                grabbedObject = grabbable;
            }
            else
            {
                // TODO: make this message a property
                // Debug.Log("Object is unavailable - may be locked");
            }
        }

    }

    public void OnTriggerExit(Collider other)
    {
        // Check if the object we stopped touching was touchable (has the touchable script on it) 
        if (touchedObject != null)
        {
            // Let the object know it is no longer being touched
            touchedObject.OnUntouched();

            // Reset the touched object
            touchedObject = null;

        }

    }

    public void OnInventoryWithdrawl(GrabbableObject grabbable)
    {
        // If we aren't already holding something
        if (grabbable != null)
        {
            // Store the current grabbable object
            grabbedObject = grabbable;

        }
    }
}
