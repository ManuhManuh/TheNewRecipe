using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject animationKey;
    [SerializeField] private List<XRGrabInteractable> lockedObjects = new List<XRGrabInteractable>();
    [SerializeField] private List<GameObject> hiddenObjects = new List<GameObject>();
    [SerializeField] private float delay = 0.5f;
    private Animator keyAnimator;

    public void Start()
    {
        // Get the key's animator
        keyAnimator = animationKey.GetComponent<Animator>();

    }

    public void OnUsed()
    {

        // Play the key insert sound
        SoundManager.PlaySound(gameObject, "KeyInsert");

        StartCoroutine(AnimateTurning(delay));


    }

    public IEnumerator AnimateTurning(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play the key turning sound
        SoundManager.PlaySound(gameObject, "KeyTurn");

        // Visually replace the socket/physics key with the animation key
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        animationKey.GetComponent<MeshRenderer>().enabled = true;

        // Play the key turning animation
        if (keyAnimator != null)
        {
            keyAnimator.SetBool("KeyTurning", true);
        }

        // Enable the locked object

        foreach (XRGrabInteractable lockedObject in lockedObjects)
        {

            lockedObject.GetComponent<XRGrabInteractable>().enabled = true;
            lockedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | 
                                                                 RigidbodyConstraints.FreezePositionY |
                                                                 RigidbodyConstraints.FreezeRotationX | 
                                                                 RigidbodyConstraints.FreezeRotationY | 
                                                                 RigidbodyConstraints.FreezeRotationZ;
        }

        foreach (GameObject hiddenObject in hiddenObjects)
        {
            hiddenObject.SetActive(true);
        }
    }
}   
