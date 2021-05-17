using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineSlot : MonoBehaviour
{
    public bool CorrectFillStatus => correctFillStatus;

    private bool shouldBeFilled;
    private bool correctFillStatus;


    private void Start()
    {
        // Check if this slot should be filled 
        shouldBeFilled = gameObject.CompareTag("CorrectWineSlot");

        // Initialize the fill status
        correctFillStatus = !shouldBeFilled;

    }

    private void OnTriggerEnter(Collider other)
    {
        // If the slot was triggered by a wine bottle
        if (other.CompareTag("WineBottle"))
        {
            // Update the correctly filled status
            correctFillStatus = shouldBeFilled;

            // Tell the Game Manager to check for the solved condition
            GameManager.OnWineSlotUpdated();

        }

    }

    private void OnTriggerExit(Collider other)
    {
        // Update the correctly filled status
        correctFillStatus = !shouldBeFilled;

        // Tell the Game Manager to check for the undoing of the solved condition
        GameManager.OnWineSlotUpdated();

    }

}
