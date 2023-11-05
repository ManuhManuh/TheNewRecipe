using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keyhole : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable handle;
    [SerializeField] private Animator keyAnimator;
    [SerializeField] private float delay;
    [SerializeField] private Key correctKey;

    public void OnKeyInserted()
    {
        
        // Play the key insert sound (the turn sound happens with the animation)
        SoundManager.PlaySound(gameObject, "KeyInsert");

        StartCoroutine(TurnAndUnlock(delay));
        
    }

    public IEnumerator TurnAndUnlock(float delay)
    {
        // Play the key turning animation
        keyAnimator.SetTrigger("TurnKey");

        yield return new WaitForSeconds(delay);

        // Play the key turning sound
        SoundManager.PlaySound(gameObject, "KeyTurn");

        float nTime = 0;
        AnimatorStateInfo animatorStateInfo;

        // wait for animation to complete
        while (nTime < 1.0f)
        {
            animatorStateInfo = keyAnimator.GetCurrentAnimatorStateInfo(0);
            nTime = animatorStateInfo.normalizedTime;
            yield return null;
        }

        // enable the grab interactable on the handle of the drawer
        handle.enabled = true;
        correctKey.OnUsed();

    }
}
