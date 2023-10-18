using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public Transform objectMesh;
    public Transform pushedPostion;
    public float maxDistanceDelta;
    public ControlledObject controlledObject;

    public void OnButtonPushed()
    {
        
        // Move the button to the pushed position
        objectMesh.position = pushedPostion.position;

        // Play the button click sound
        SoundManager.PlaySound(gameObject, "WallButtonClick");

        // Perform the action that the button is supposed to control
        controlledObject.OnPressed();

    }

    public void OnButtonReleased()
    {
       
        // Return the button to the original position
        objectMesh.localPosition = Vector3.zero;

        // Play the button click sound
        SoundManager.PlaySound(gameObject, "WallButtonClick");

        // Perform the action that releasing the button is supposed to control
        controlledObject.OnReleased();

    }

   

}
