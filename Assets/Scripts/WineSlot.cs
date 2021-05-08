using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineSlot : MonoBehaviour
{
    private SpringJoint springJoint;
    private bool hasCorrectFillStatus;
    private WineBottle wineBottle;
    private bool shouldBeFilled;
    private Vector3 homePosition;
    private Quaternion homeRotation;

    private void Start()
    {
        shouldBeFilled = gameObject.CompareTag("CorrectWineSlot");
        homePosition = transform.position;
        homeRotation = transform.rotation;

    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the slot was triggered by a wine bottle
        var isWineBottle = other.TryGetComponent<WineBottle>(out wineBottle);

        // If it was triggered by a wine bottle
        if (isWineBottle)
        {
            // If the wine bottle doesn't already have a current slot
            if (!wineBottle.InAWineSlot)
            {
                // Let the wine bottle know where it should move to and if the slot should be filled
                wineBottle.OnSlotEntered(homePosition, homeRotation, shouldBeFilled);
            }
 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the slot was exited by a wine bottle
        var isWineBottle = other.TryGetComponent<WineBottle>(out wineBottle);
        
        if (isWineBottle)
        {
            // Let the wine bottle know it is no longer in a slot
            wineBottle.OnSlotExited();
        }
        
    }
}
