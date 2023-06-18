using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{
    [SerializeField] private Transform sampleKey;
    [SerializeField] private List<XRGrabInteractable> lockedObjects = new List<XRGrabInteractable>();
    [SerializeField] private List<GameObject> hiddenObjects = new List<GameObject>();

    private Animator keyAnimator;
    private bool used = false;

    public void Start()
    {
        // Get the key's animator
        keyAnimator = gameObject.GetComponent<Animator>();

    }

    public void OnUsed()
    {
        // Play the key insert sound
        SoundManager.PlaySound(gameObject, "KeyInsert");

        // Play the key turning sound
        SoundManager.PlaySound(gameObject, "KeyTurn");

        // Play the key turning animation
        if (keyAnimator != null)
        {
            keyAnimator.SetBool("KeyTurning", true);
        }

        // Enable the locked object

        foreach(XRGrabInteractable lockedObject in lockedObjects)
        {
            lockedObject.GetComponent<XRGrabInteractable>().enabled = true;
        }

        foreach (GameObject hiddenObject in hiddenObjects)
        {
            hiddenObject.SetActive(true);
        }


    }

}   
