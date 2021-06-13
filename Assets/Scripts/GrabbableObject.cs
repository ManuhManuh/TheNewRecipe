using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public bool ObjectUsed
    {
        get 
        { 
            return objectUsed; 
        }
        set 
        { 
            objectUsed = value; 
        }
    }

    public bool ObjectLocked
    {
        get
        {
            return objectLocked;
        }
        set
        {
            objectLocked = value;
        }
    }
    protected Rigidbody rigidBody;
    private int inventoryObjectLayer;
    private Vector3 objectLocalPosition;
    private Quaternion objectLocalRotation;
    private bool objectUsed;
    private bool objectLocked;

    public virtual void Start()
    {
        objectUsed = false;
        inventoryObjectLayer = LayerMask.NameToLayer("InventoryObject");
    }

    public virtual void OnGrab(ObjectGrabber grabber)
    {
        if (!objectUsed && !objectLocked)
        {
            // See if the object has a rigidbody
            gameObject.TryGetComponent<Rigidbody>(out rigidBody);

            // If there is no rigidbody on the item (because it was in a drawer for example)
            if (rigidBody == null)
            {
                gameObject.AddComponent<Rigidbody>();
                rigidBody = GetComponent<Rigidbody>();
            }

            // Parent this object to the grabber
            transform.SetParent(grabber.transform);

            // Turn off physics
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
        }
    }

    public virtual void OnDrop()
    {
        // Check for rigidbody - sometimes fake drop happens without pickup happening first and no rigidbody is added
        gameObject.TryGetComponent<Rigidbody>(out rigidBody);

        if (rigidBody != null)
        {
            // Record the local position and rotation
            objectLocalPosition = transform.localPosition;
            objectLocalRotation = transform.localRotation;

            if (transform.gameObject.layer == inventoryObjectLayer)
            {
                // See if the object has been used for its intended purpose
                if (objectUsed)
                {
                    // Let the Inventory Manager know the object has been used
                    InventoryManager.instance.OnInventoryObjectUsed(this);

                    // Note: not unparented here as the using process parents it to whatever used it
                }
                else
                {
                    // Unparent the object
                    transform.SetParent(null);

                    // Let the Inventory Manager know the object has been acquired for inventory
                    InventoryManager.instance.OnInventoryObjectAcquired(this, objectLocalPosition, objectLocalRotation);
                }

                // Skip the physics restoration either way
            }
            else
            {
                // Unparent the object
                transform.SetParent(null);

                // Turn on physics
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
            }
            
        }
        else
        {
            // All objects that have been previously picked up should have rigidbodies, so this would be a fake drop
            // Debug.Log($"Fake drop on {gameObject.name}");
        }

    }

    public virtual void OnTriggerEnd()
    {
    }
}
