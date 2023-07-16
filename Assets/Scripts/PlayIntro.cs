using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PlayIntro : MonoBehaviour
{
   
    [SerializeField] Button skipButton;
    private bool previouslyPlayed;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        previouslyPlayed = PlayerPrefs.HasKey("Played");

        if (previouslyPlayed)
        {
        
            skipButton.enabled = previouslyPlayed;
        }

        SoundManager.PlayMusic("Alex Mason - Prisoner", 0f, 0.1f);

        // Note: chapter audio and text are now played with a playable director attached to the canvas

    }

    // Use these methods with the signal emitters on the timeline
    public void PreloadFirstChapter()
    {
        Debug.Log("Preload chapter requested");
        SceneConductor.instance.PreLoadChapter(SceneConductor.SceneIndex.Chapter01);
    }

    public void EndIntro()
    {
        Debug.Log("End Intro requested");
        PlayableDirector timeline = FindObjectOfType<PlayableDirector>();
        if(timeline != null)
        {
            timeline.Stop();
        }

        PlayerPrefs.SetString("Played", "True");
        SceneConductor.instance.ActivateChapter(SceneConductor.SceneIndex.Chapter01);
        
    }

    public void EnableSkipButton()
    {
        Debug.Log("Skip enabled");
        skipButton.enabled = true;
    }

   
   
}
