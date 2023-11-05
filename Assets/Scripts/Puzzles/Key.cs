using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{
    XRGrabInteractable key;

    public void Start()
    {

        key = gameObject.GetComponent<XRGrabInteractable>();
    }

    public void OnUsed()
    {
        // change the interaction layer so key is not reused
        key.interactionLayers = InteractionLayerMask.GetMask("UsedKey");
        gameObject.GetComponent<Rigidbody>().isKinematic = false;   // animation finished before this method called
        
    }

}   
