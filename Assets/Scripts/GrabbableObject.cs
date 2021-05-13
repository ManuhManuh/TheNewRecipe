using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    //public GameObject gripTarget;

    public bool AvailableToGrab => availableToGrab;
    public bool ObjectUsed => objectUsed;

    protected Rigidbody rigidBody;
    private int inventoryObjectLayer;
    private Vector3 objectLocalPosition;
    private Quaternion objectLocalRotation;
    private bool availableToGrab;
    private bool objectUsed;

    private void Awake()
    {
        // Default to available: Objects that should start unavailable will have update this property in their Start method
        availableToGrab = true;
        objectUsed = false;
}
    public virtual void Start()
    {
        inventoryObjectLayer = LayerMask.NameToLayer("InventoryObject");
    }

    public virtual void OnGrab(ObjectGrabber grabber)
    { 
        //Debug.Log($"{ this.name} was grabbed");
        gameObject.TryGetComponent<Rigidbody>(out rigidBody);

        // If there is no rigidbody on the item (because it was in a drawer for example)
        if (rigidBody == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rigidBody = GetComponent<Rigidbody>();
        }

        // Child this object to the grabber
        transform.SetParent(grabber.transform);
       
        // Turn off physics
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }

    public virtual void OnDrop()
    {
        //Debug.Log($"{ this.name} was dropped");
        gameObject.TryGetComponent<Rigidbody>(out rigidBody);

        // Record the local position and rotation
        objectLocalPosition = transform.localPosition;
        objectLocalRotation = transform.localRotation;

         // Unparent the object
        transform.SetParent(null);

        if (transform.gameObject.layer == inventoryObjectLayer)
        {
            // Let the Inventory Manager know the object has been acquired for inventory
            InventoryManager.instance.OnInventoryObjectAcquired(this, objectLocalPosition, objectLocalRotation);

            // Skip the physics restoration
            return;
        }

        if (rigidBody != null)
        {
            // Turn on physics
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
        }

    }

    public virtual void OnDrop(bool used)
    {
        //Debug.Log($"{ this.name} was dropped");
        gameObject.TryGetComponent<Rigidbody>(out rigidBody);

        if (!used)
        {
            // Record the local position and rotation (used items will be placed in position where they are used)
            objectLocalPosition = transform.localPosition;
            objectLocalRotation = transform.localRotation;

            // Unparent the object if it is not used (used objects will be parented to what they were used on)
            transform.SetParent(null);
        }
        
        if (transform.gameObject.layer == inventoryObjectLayer)
        {
            // See if the object has been used for its intended purpose
            if (used)
            {
                // Let the Inventory Manager know the object has been used
                InventoryManager.instance.OnInventoryObjectUsed(this);
            }
            else
            {
                // Let the Inventory Manager know the object has been acquired for inventory
                InventoryManager.instance.OnInventoryObjectAcquired(this, objectLocalPosition, objectLocalRotation);
            }

            // Skip the physics restoration
            return;
        }

        if (rigidBody != null)
        {
            // Turn on physics
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
        }
    }

    public void OnAvailabilityChanged(bool newState)
    {
        // This will be called when an object becomes available or unavailable (i.e., unlocked or locked)
        availableToGrab = newState;
    }

    public virtual void OnTriggerEnd()
    {
    }
}
