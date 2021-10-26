using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSequence : MonoBehaviour
{
    public GameObject realCask;
    public GameObject animatedCask;
    public GameObject animatedLid;
    public AnimationClip openDoor;
    public GameObject interiorTeleportTarget;
    public GameObject hiddenDoor;
    public GameObject splashPanel;
    public GameObject stayTunedMessage;
    public float fadeDuration;

    private Animator barrelAnimator;
    private bool sequenceStarted = false;

    private void Start()
    {
        barrelAnimator = animatedLid.GetComponent<Animator>();
    }
    void Update()
    {
        if (GameManager.allPuzzlesSolved && !sequenceStarted)
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

        if (Input.GetKeyDown(KeyCode.Space) && !sequenceStarted)    //TODO: remove this in production - used for testing end sequence
        {
            sequenceStarted = true;

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


}
