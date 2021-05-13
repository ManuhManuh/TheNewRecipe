using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public int capacity;
    public TMP_Text label;
    public string itemName;
    public ObjectGrabber defaultObjectGrabber;

    private int countOfItems;
    private Vector3 objectPosition;
    private Quaternion objectRotation;
    private GrabbableObject objectToDispense;
    private Vector3 objectReturnPosition;
    private Quaternion objectReturnRotation;

    private void Start()
    {
        // Find the sample object among child objects that shares a tag
        foreach(Transform child in transform)
        {
            if (child.CompareTag(gameObject.tag))
            {
                // Record the position and rotation of the sample object
                objectPosition = child.position;
                objectRotation = child.rotation;

                // Destroy the sample object
                Destroy(child.gameObject);
            }
        }

    }

    public void OnObjectDeposited(GrabbableObject inventoryObject, Vector3 objectLocalPosition, Quaternion objectLocalRotation)
    {
        // Increment the object count
        countOfItems++;

        // Record the current position and rotation of the object so it can be returned there
        objectReturnPosition = objectLocalPosition;
        objectReturnRotation = objectLocalRotation;

        // Turn off physics
        inventoryObject.GetComponent<Rigidbody>().isKinematic = true;
        inventoryObject.GetComponent<Rigidbody>().useGravity = false;

        // Place the ojbect in the slot so it is viewable on the inventory camera
        inventoryObject.transform.position = objectPosition;
        inventoryObject.transform.rotation = objectRotation;

        // Parent the object to the slot
        inventoryObject.transform.SetParent(gameObject.transform);

        // Update the text on the slot with the new total
        label.text = ($"{gameObject.tag.ToString()} ({countOfItems})");
    }

    public void OnObjectWithdrawn()
    {

        if(countOfItems > 0)
        {
            // Adjust the item count
            countOfItems--;

            // Place the object in the player's hand   
            foreach (Transform child in transform)
            {
                if (child.CompareTag(gameObject.tag))
                {
                    objectToDispense = child.GetComponent<GrabbableObject>();
                }
            }

            // Implement the whole grabbing scenario with the default grabber
            objectToDispense.OnGrab(defaultObjectGrabber);
            defaultObjectGrabber.OnInventoryWithdrawl(objectToDispense);

            //objectToDispense.transform.localPosition = objectReturnPosition;
            objectToDispense.transform.localPosition = Vector3.zero;
            //objectToDispense.transform.localRotation = objectReturnRotation;
            objectToDispense.transform.localRotation = Quaternion.identity;

            // Check to see if we have removed the last of that item
            if (countOfItems == 0)
            {
                // Remove the caption from the block
                label.text = (" ");
            }
            else
            {
                // Update the text on the slot with the name and new total (if multiples exist)
                if(capacity == 1)
                {
                    label.text = ($"{gameObject.tag.ToString()}");
                }
                else
                {
                    label.text = ($"{gameObject.tag.ToString()} ({countOfItems})");
                } 
            }
            
        }
        else
        {
            Debug.Log("Not enough of this item to present");
        }
        
    }

}
