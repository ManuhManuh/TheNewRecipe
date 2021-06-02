using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public Transform objectMesh;
    public Transform pushedPostion;
    public float maxDistanceDelta;
    public ControlledObject controlledObject;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered: {other.name}");
        if (other.tag == "Player")
        {
            // Move the button to the pushed position
            objectMesh.position = pushedPostion.position;

            // Play the button click sound
            SoundManager.PlaySound(gameObject, "WallButtonClick");

            // Perform the action that the button is supposed to control
            controlledObject.OnPressed();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited: {other.name}");
        if (other.tag == "Player")
        {
            // Return the button to the original position
            objectMesh.localPosition = Vector3.zero;

            // Play the button click sound
            SoundManager.PlaySound(gameObject, "WallButtonClick");

            // Perform the action that releasing the button is supposed to control
            controlledObject.OnReleased();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Trigger staying: {other.name}");
    }

}
