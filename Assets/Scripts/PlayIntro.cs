using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayIntro : MonoBehaviour
{
    public List<Paragraph> paragraphs = new List<Paragraph>();
    public Canvas canvas;

    private void Start()
    {
        // Note: make sure paragraphs are in order in the inspector
    
            StartCoroutine(PlayParagraphs());

    }


    private IEnumerator PlayParagraphs()
    {
        foreach(Paragraph para in paragraphs)
        {
            // enable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = true;
            
            //TODO: Fade this in

            // play the audio clip
            SoundManager.PlaySound(para.sourceObject, para.audioClip.name);

            // Wait until the narration is finished
            yield return new WaitForSeconds(para.duration);

            // disable the paragraph text
            para.GetComponent<TextMeshProUGUI>().enabled = false;

            //TODO: Fade this out
        }
        

    }

}
