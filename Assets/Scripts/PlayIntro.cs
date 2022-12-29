using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayIntro : MonoBehaviour
{
    public List<Paragraph> paragraphs = new List<Paragraph>();
    public Canvas canvas;
    public string buttonSkipIntroInputName;
    public TMP_Text skipMessage;

    private bool previouslyPlayed;

    private void Start()
    {
        previouslyPlayed = PlayerPrefs.HasKey("Played");

        // Note: make sure paragraphs are in order in the inspector

        if (previouslyPlayed)
        {
            // move StartCoroutine here when testing is done
            skipMessage.enabled = previouslyPlayed;
        }


        StartCoroutine(PlayParagraphs());

    }

    public void Update()
    {
        if (Input.GetButtonDown(buttonSkipIntroInputName))
        {
            StartCoroutine(EndIntro());
        }
    }
    private IEnumerator PlayParagraphs()
    {
        // Start the ambient game music, minimum duration 0, volume 0.5
        SoundManager.PlayMusic("Alex Mason - Prisoner",0f,0.1f);

        // Notify scene contol that first chapter can start pre-loading
        SceneControl.instance.ReadyForPreload = true;

        foreach (Paragraph para in paragraphs)
        {
            // enable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = true;
            
            //TODO: Fade this in

            // play the audio clip
            SoundManager.PlaySound(para.sourceOfSound, para.audioClip.name);

            // Wait until the narration is finished
            yield return new WaitForSeconds(para.duration);

            // disable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = false;

            //TODO: Fade this out

        }

        PlayerPrefs.SetString("Played", "True");

        StartCoroutine(EndIntro());
    }

    private IEnumerator EndIntro()
    {

        // Notify scene control that intro has finished, so first chapter can be activated
        SceneControl.instance.IntroFinished = true;

        yield return new WaitForSeconds(3.0f);
        // Return backgroud music to normal volume
        SoundManager.PlayMusic("Alex Mason - Prisoner", 0f, 1.0f);
    }

}
