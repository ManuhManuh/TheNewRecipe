using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSequence : MonoBehaviour
{
    [SerializeField] private GameObject realCask;
    [SerializeField] private GameObject animatedCask;
    [SerializeField] private GameObject animatedLid;
    [SerializeField] private AnimationClip openDoor;
    [SerializeField] private GameObject interiorTeleportTarget;
    [SerializeField] private GameObject hiddenDoor;
    [SerializeField] private GameObject splashPanel;
    [SerializeField] private GameObject stayTunedMessage;
    [SerializeField] private float fadeDuration;

    private Animator barrelAnimator;
    private bool sequenceStarted = false;

    private void Start()
    {
        barrelAnimator = animatedLid.GetComponent<Animator>();
    }
    public void OnWin()
    {

            // Disable Cask B 
            realCask.SetActive(false);

            // Enable Barrel Door
            animatedCask.SetActive(true);

            // Play animation
            barrelAnimator.SetBool("DoorOpening", true);

            // Play sound effect
            SoundManager.PlaySound(gameObject, "DumbwaiterDoorOpen");

            // Make hidden door visible
            hiddenDoor.SetActive(true); //Figure out how to fade in .. really abrupt

            // Activate teleport target inside barrel
            interiorTeleportTarget.SetActive(true);
       

    }


}
