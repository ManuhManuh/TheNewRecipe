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
        if (other.tag == "Player")
        {
            // Move the button to the pushed position
            objectMesh.position = pushedPostion.position;

            // Perform the action that the button is supposed to control
            controlledObject.OnPressed();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Return the button to the original position
            objectMesh.localPosition = Vector3.zero;

            // Perform the action that releasing the button is supposed to control
            controlledObject.OnReleased();

        }
    }
}
