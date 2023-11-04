using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keyhole : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable handle;
    [SerializeField] private GameObject animationKey;
    [SerializeField] private GameObject travellingKey;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private Animator keyAnimator;

    private GameObject insertedKey;
    public void OnKeyInserted()
    {
        // enable the XR Grab interactable on the handle of the drawer
        handle.enabled = true;

        // Play the key insert sound
        SoundManager.PlaySound(gameObject, "KeyInsert");
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
        insertedKey = objName.transform.gameObject;

        animationKey.GetComponent<MeshRenderer>().enabled = true;
        Destroy(insertedKey);

        StartCoroutine(AnimateTurning(delay));
    }

    public IEnumerator AnimateTurning(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play the key turning sound
        SoundManager.PlaySound(gameObject, "KeyTurn");

        // Play the key turning animation
        keyAnimator.SetBool("KeyTurning", true);

        yield return new WaitForSeconds(2.2f);  // animation is 2 seconds long

        // switch the keys back and make the real key not grabbable

        travellingKey.GetComponent<MeshRenderer>().enabled = true;
        Destroy(animationKey);

    }
}
