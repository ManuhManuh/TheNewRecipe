using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PlayIntro : MonoBehaviour
{
    [SerializeField] Animator skipButton;
    [SerializeField] AudioClip mainMusicTrack;
    [SerializeField] float musicVolume;

    private bool firstChapterLoaded = false;
    private bool skipButtonDisplayed = false;
    
    private void Update()
    {
        if (!firstChapterLoaded)
        {
            firstChapterLoaded = SceneControl.instance.ChapterLoaded;

        }
        else if (!skipButtonDisplayed)
        {
            skipButtonDisplayed = true;
            skipButton.SetBool("FadeButtonIn", true);

        }

        
    }

    public void StartMainMusic()
    {
        SoundManager.PlayMusic(mainMusicTrack.name,0,musicVolume);
    }

}
