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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.allPuzzlesSolved )
        {
            // Disable Cask B
            realCask.SetActive(false);

            // Enable Barrel Door
            animatedCask.SetActive(true);

            // Play animation
            barrelAnimator = animatedLid.GetComponent<Animator>();
            barrelAnimator.SetBool("DoorOpening", true);

            // Play sound effect
            SoundManager.PlaySound(gameObject, "DumbwaiterDoorOpen");

            // Make hidden door visible
            hiddenDoor.SetActive(true); //Figure out how to fade in .. really abrupt

            // Activate teleport target inside barrel
            interiorTeleportTarget.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Disable Cask B
            realCask.SetActive(false);

            // Enable Barrel Door
            animatedCask.SetActive(true);

            // Play animation
            barrelAnimator = animatedLid.GetComponent<Animator>();
            barrelAnimator.SetBool("DoorOpening", true);

            // Play sound effect
            SoundManager.PlaySound(gameObject, "DumbwaiterDoorOpen");

            // Make hidden door visible
            hiddenDoor.SetActive(true); //Figure out how to fade in .. really abrupt

            // Activate teleport target inside barrel
            interiorTeleportTarget.SetActive(true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision on handle detected");

        // Show the stay tuned message
        stayTunedMessage.SetActive(true);

        // Request the Game Manager to fade the panel to black and play credits
        bool fadeOut = true;
        
        SceneControl.OnPanelFadeRequest(splashPanel, fadeDuration, SceneControl.SceneAction.Credits, fadeOut);
    }

   
}
