using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject animationKey;
    [SerializeField] private GameObject travellingKey;
    [SerializeField] private float delay = 0.5f;
    private Animator keyAnimator;

    public void Start()
    {
        // Get the animation key's animator
        if(animationKey != null)
        {
            keyAnimator = animationKey.GetComponent<Animator>();
          
        }

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
        
        // Play the key turning animation
        if (keyAnimator != null)
        {
            animationKey.GetComponent<MeshRenderer>().enabled = true;
            keyAnimator.SetBool("KeyTurning", true);
        }

        yield return new WaitForSeconds(2.2f);  // animation is 2 seconds long

        // switch the keys back and make the real key not grabbable

        travellingKey.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        animationKey.GetComponent<MeshRenderer>().enabled = false;

    }
}   
