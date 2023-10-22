using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // panels with different menus
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject instructions;
    [SerializeField] GameObject intro;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject ending;
    [SerializeField] GameObject credits;

    [SerializeField] PlayableDirector introTimeline;

    private GameObject currentPanel;

    private void Start()
    {
        currentPanel = mainMenu;

    }

    public void ReturnToMain()
    {
        currentPanel.SetActive(false);
        mainMenu.SetActive(true);
        currentPanel = mainMenu;

    }

    public void ShowInstructions()
    {
        currentPanel.SetActive(false);
        instructions.SetActive(true);
        currentPanel = instructions;

    }

    public void PlayIntro()
    {
        currentPanel.SetActive(false);
        intro.SetActive(true);
        introTimeline.Play();
        currentPanel = intro;
    }

    public void ShowPauseMenu()
    {
        Debug.Log("Show pause menu");
        currentPanel.SetActive(false);
        pause.SetActive(true);
        currentPanel = pause;
    }

    public void ResumeChapter()
    {
        currentPanel.SetActive(false);
        SceneControl.instance.ResumeChapter();
        currentPanel = mainMenu;
    }

    public void SkipIntro()
    {
        introTimeline.Stop();
        currentPanel.SetActive(false);
        SceneControl.instance.ActivateLoadedScene();
        currentPanel = mainMenu;
    }


    public void ShowCredits()
    {
        currentPanel.SetActive(false);
        credits.SetActive(true);
        currentPanel = credits;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
