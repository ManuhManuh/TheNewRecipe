using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameObject inventoryDisplay;
    public string buttonInventoryDisplayInputName;
    public string buttonInventorySelectInputName;
    public string thumbstickAdvanceInputName;
    public float thumbstickAdvanceThreshold;

    public List<GameObject> highlights = new List<GameObject>();
    public List<InventorySlot> slots = new List<InventorySlot>();

    public bool InventoryOpen => inventoryActive;
    private bool inventoryActive;

    private int nextIndex;
    private int advance;

    private void Awake()
    {
        // Are there any other game managers yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Inventory Manager");
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the A button is pressed
        //if (Input.GetButtonDown(buttonInventoryDisplayInputName))
        //{
        //    // Toggle the inventory display in/out of view
        //    inventoryDisplay.SetActive(!inventoryDisplay.activeSelf);
        //}

        //// If the thumbstick is being pushed sideways while inventory is active
        //if (Input.GetAxis(thumbstickAdvanceInputName) < -thumbstickAdvanceThreshold && inventoryDisplay.activeSelf)
        //{
        //    // Flag the rotation of the player to be 90 degrees to the left
        //    advance = -1;
        //}
        //else
        //{
        //    // If the thumbstick is being pushed right (player want to turn right)
        //    if (Input.GetAxis(thumbstickAdvanceInputName) > thumbstickAdvanceThreshold && inventoryDisplay.activeSelf)
        //    {
        //        // Flag the rotation of the player to be 90 degrees to the right
        //        advance = 1;
        //    }
        //    else
        //    {
        //        // Check if the thumbstick has been released
        //        if (Input.GetAxis(thumbstickAdvanceInputName) == 0 && !(advance == 0) && inventoryDisplay.activeSelf)
        //        {
        //            // Cycle to next highlight
        //            for (int i = 0; i < highlights.Count; i++)
        //            {
        //                if (highlights[i].activeSelf)
        //                {
        //                    nextIndex = i + 1;
        //                    if (nextIndex == highlights.Count)
        //                    {
        //                        nextIndex = 0;
        //                    }
        //                    highlights[i].SetActive(false);
        //                }
        //            }
        //            highlights[nextIndex].SetActive(true);
        //            advance = 0;
        //        }
        //    }
        //}

        //// If the B button is pressed
        //if (Input.GetButtonDown(buttonInventorySelectInputName))
        //{
        //    // Cycle to next highlight
        //    for (int i = 0; i < highlights.Count; i++)
        //    {
        //        if (highlights[i].activeSelf)
        //        {
        //            slots[i].OnObjectWithdrawn();
        //        }
        //    }

        //}

        //inventoryActive = inventoryDisplay.activeSelf;
    }

    public void OnInventoryObjectAcquired(InventoryObject inventoryObject, Vector3 objectLocalPosition, Quaternion objectLocalRotation)
    {
        // Figure out which slot the object goes in
        for (int i = 0; i < slots.Count; i++)
        {

            if (slots[i].CompareTag(inventoryObject.tag))
            {
                // Tell the slot the object has been deposited
                slots[i].OnObjectDeposited(inventoryObject, objectLocalPosition, objectLocalRotation);
            }
        }
        

    }

    public void OnInventoryObjectUsed(InventoryObject inventoryObject)
    {
        // Currently does nothing, except avoids being put back in inventory

    }
}
