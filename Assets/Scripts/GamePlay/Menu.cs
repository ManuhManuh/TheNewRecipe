using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    public void MenuChoiceOnClick(string selection)
    {
   
            switch (selection)
            {
                case "InstructionsButton":
                    {
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Instructions);
                        return;
                    }

                case "PlayButton":
                    {
                        // resume button is only present in main menu
                        if (resumeButton != null) resumeButton.gameObject.SetActive(true);
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Intro);
                        return;
                    }

                case "ResumeButton":
                    {
                        SceneConductor.instance.CloseMainMenu();
                        return;
                    }

                case "CreditsButton":
                    {
                    Debug.Log("Credits requested");
                    SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Credits);
                        return;
                    }

                case "ExitButton":
                    {
                        SceneConductor.instance.ExitGame();
                        return;
                    }
                case "MainMenuButton":  // NOTE: This is NOT used when chapter is open for pause menu
                    {
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.MainMenu);
                        return;
                    }
                case "SkipButton":
                    {
                        PlayableDirector timeline = FindObjectOfType<PlayableDirector>();
                        if(timeline != null)
                        {
                            timeline.Stop();
                        }
                        SceneConductor.instance.ActivateChapter(SceneConductor.SceneIndex.Chapter01);
                        return;
                    }
            }

    }

}
