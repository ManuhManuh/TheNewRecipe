using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turning : MonoBehaviour
{
    public string thumbstickTurnInputName;
    public float thumbstickRightThreshold;
    public float thumbstickLeftThreshold;
    public Transform player;

    private float rotation;

    void Update()
    {
        // If the thumbstick is being pushed left (player wants to turn left) 
        if (Input.GetAxis(thumbstickTurnInputName) < thumbstickLeftThreshold)
        {
            // Flag the rotation of the player to be 30 degrees to the left
            rotation = -30;
        }
        else
        {
            // If the thumbstick is being pushed right (player want to turn right)
            if (Input.GetAxis(thumbstickTurnInputName) > thumbstickRightThreshold)
            {
                // Flag the rotation of the player to be 30 degrees to the right
                rotation = 30;
            }
            else
            {
                // Check if the thumbstick has been released
                if (Input.GetAxis(thumbstickTurnInputName) == 0 && !(rotation == 0))
                {
                    // Perform the rotation
                    player.transform.eulerAngles = new Vector3(
                        player.transform.eulerAngles.x,
                        player.transform.eulerAngles.y + rotation,
                        player.transform.eulerAngles.z);

                    // Reset the rotation flag
                    rotation = 0;
                }
            }
        }
        
    }
}

