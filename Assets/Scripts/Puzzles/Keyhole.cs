using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keyhole : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable handle;
    public void OnKeyInserted()
    {
        // enable the XR Grab interactable on the handle of the drawer
        handle.enabled = true;

    }
}
