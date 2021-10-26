using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayOutro : MonoBehaviour
{
    public int viewDuration;

    private bool playing;
    private int secondsLeft;

    private void Awake()
    {
        UnityEngine.XR.InputTracking.Recenter();
    }

    private void Start()
    {

        // Note: make sure paragraphs are in order in the inspector
        playing = true;
        secondsLeft = viewDuration;
        StartCoroutine(PlayStayTuned());

    }


    private IEnumerator PlayStayTuned()
    {
        while (playing)
        {
            // Wait until the narration is finished
            yield return new WaitForSeconds(1);
            secondsLeft--;
            playing = secondsLeft > 0;
        }
        
        // Notify scene control that outro has finished, so credits can be activated
        SceneControl.instance.OutroFinished = true;

    }
}
