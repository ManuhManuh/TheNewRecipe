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
        if (GameManager.allPuzzlesSolved)
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


    public void CallFadeRequest()
    {
        // Show the stay tuned message
        stayTunedMessage.SetActive(true);

        // Request the Game Manager to fade the panel to black and play credits
        bool fadeOut = true;

        SceneControl.OnPanelFadeRequest(splashPanel, fadeDuration, SceneControl.SceneAction.Credits, fadeOut);
        //TestFadeRequest(splashPanel, fadeDuration, fadeOut);
    }


    public void TestFadeRequest(GameObject panelToFade, float fadeDuration, bool fadeOut)
    {
        StartCoroutine(FadePanel(panelToFade, fadeDuration, fadeOut));
    }

    private IEnumerator FadePanel(GameObject panelToFade, float fadeDuration, bool fadeOut)
    {
        //float fadeTimer = 0;
        Image panelImage = panelToFade.GetComponent<Image>();
        Color newColour = panelImage.color;

        if (fadeOut)
        {
            // Fade the panel to black

            for (float i = 0; i <= fadeDuration; i += Time.deltaTime)
            {
                newColour.a = i;
                panelImage.color = newColour;

                yield return null;
            }

        }
        else
        {
            // Fade the panel to invisible

            for (float i = fadeDuration; i >= 0; i -= Time.deltaTime)
            {
                newColour.a = i;
                panelImage.color = newColour;

                yield return null;
            }

        }

        // Let panel stay for three seconds
        yield return new WaitForSeconds(3f);

        Debug.Log("Credits would be playing now");

        yield return null;
    }
}
