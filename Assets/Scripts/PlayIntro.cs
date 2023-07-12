using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayIntro : MonoBehaviour
{
    public List<Paragraph> paragraphs = new List<Paragraph>();
    public Canvas canvas;
    public Button skipButton;

    private bool previouslyPlayed;
    private bool preloadBegun;

    private void Start()
    {
        preloadBegun = false;

        previouslyPlayed = PlayerPrefs.HasKey("Played");

        // Note: make sure paragraphs are in order in the inspector

        if (previouslyPlayed)
        {
            // move StartCoroutine here when testing is done
            skipButton.enabled = previouslyPlayed;
        }


        StartCoroutine(PlayParagraphs());

    }

    public void SkipIntro()
    {
        Debug.Log("Clicked SkipIntro (Play Intro)");
        SceneConductor.instance.ActivateChapter(SceneConductor.SceneIndex.Chapter01);
    }

    private IEnumerator PlayParagraphs()
    {
        // Start the ambient game music, minimum duration 0, volume 0.5
        SoundManager.PlayMusic("Alex Mason - Prisoner",0f,0.1f);

        // Notify scene contol that first chapter can start pre-loading
        // SceneControl.instance.ReadyForPreload = true;
        

        foreach (Paragraph para in paragraphs)
        {
            // enable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = true;
            
            //TODO: Fade this in

            // play the audio clip
            SoundManager.PlaySound(para.sourceOfSound, para.audioClip.name);

            // wait until the main menu is unloaded
            if (SceneManager.GetSceneByName("MainMenu").isLoaded)
            {
                yield return null;
            }

            // start preloading the chapter if this is the first paragraph
            if (!preloadBegun)
            {
                SceneConductor.instance.PreLoadChapter(SceneConductor.SceneIndex.Chapter01);
                preloadBegun = true;
            }

            // Wait until the narration is finished
            yield return new WaitForSeconds(para.duration);

            // disable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = false;

            //TODO: Fade this out

        }

        PlayerPrefs.SetString("Played", "True");

        SceneConductor.instance.ActivateChapter(SceneConductor.SceneIndex.Chapter01);
    }

}
