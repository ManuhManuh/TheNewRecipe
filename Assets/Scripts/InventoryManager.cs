using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameObject inventoryDisplay;
    public string buttonRightPrimaryInputName;
    public string buttonLeftPrimaryInputName;
    public string thumbstickAdvanceInputName;
    public float thumbstickAdvanceThreshold;

    public List<GameObject> highlights = new List<GameObject>();

    public bool InventoryOpen => inventoryActive;
    private bool inventoryActive;

    private int nextIndex;
    private int advance;

    private void Awake()
    {
        // Are there any other inventory managers yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Inventory Manager");
        }
        else
        {
            instance = this;
        }
        inventoryActive = inventoryDisplay.activeSelf;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the A button is pressed
        if (Input.GetButtonDown(buttonRightPrimaryInputName))
        {
            // Toggle the inventory display in/out of view
            inventoryDisplay.SetActive(!inventoryDisplay.activeSelf);
        }

        // If the thumbstick is being pushed sideways while inventory is active
        if (Input.GetAxis(thumbstickAdvanceInputName) < -thumbstickAdvanceThreshold)
        {
            // Flag the rotation of the player to be 90 degrees to the left
            advance = -1;
        }
        else
        {
            // If the thumbstick is being pushed right (player want to turn right)
            if (Input.GetAxis(thumbstickAdvanceInputName) > thumbstickAdvanceThreshold)
            {
                // Flag the rotation of the player to be 90 degrees to the right
                advance = 1;
            }
            else
            {
                // Check if the thumbstick has been released
                if (Input.GetAxis(thumbstickAdvanceInputName) == 0 && !(advance == 0))
                {
                    // Cycle to next highlight
                    for (int i = 0; i < highlights.Count; i++)
                    {
                        if (highlights[i].activeSelf)
                        {
                            nextIndex = i + 1;
                            if (nextIndex == highlights.Count)
                            {
                                nextIndex = 0;
                            }
                            highlights[i].SetActive(false);
                        }
                    }
                    highlights[nextIndex].SetActive(true);
                    advance = 0;
                }
            }
        }

        // If the B button is pressed
        if (Input.GetButtonDown(buttonLeftPrimaryInputName))
        {
            // If the object is in the inventory, give it to the player
            
        }

        inventoryActive = inventoryDisplay.activeSelf;
    }

    public void OnObjectDeposited()
    {
        // Make the object visible in the inventory view

        // Make the object invisible in the scene

        // Tell the appropriate slot that it now has an object
    }

    public void OnObjectWithdrawn()
    {
        // Find out which object was selected to withdraw
        // Call whatever grabbable script is required by the specific object (ie with joint or not)
        // Tell the appropriate slot that an object has been removed
    }
}
