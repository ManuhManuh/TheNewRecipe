using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Menu : MonoBehaviour
{
    public void MenuChoiceOnClick(string selection)
    {
   
            switch (selection)
            {
                case "InstructionsButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.Instructions);
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Instructions);
                        return;
                    }

                case "PlayButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.PlayIntro);
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Intro);
                        return;
                    }

                case "ResumeButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.Resume);
                        SceneConductor.instance.CloseMainMenu();
                        return;
                    }

                case "CreditsButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.Credits);
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Credits);
                        return;
                    }

                case "ExitButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.Exit);
                        SceneConductor.instance.ExitGame();
                        return;
                    }
                case "MainMenuButton":  // NOTE: This is NOT used when chapter is open for pause menu
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.Menu);
                        SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.MainMenu);
                        return;
                    }
                case "SkipButton":
                    {
                        // SceneControl.OnMenuSelection(SceneControl.SceneAction.PlayChapter);
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
