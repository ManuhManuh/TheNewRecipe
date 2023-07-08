using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void MenuChoiceOnClick(string selection)
    {
   
            switch (selection)
            {
                case "InstructionsButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Instructions);
                        return;
                    }

                case "PlayButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.PlayIntro);
                        return;
                    }

                case "ResumeButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Resume);
                        return;
                    }

                case "CreditsButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Credits);
                        return;
                    }

                case "ExitButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Exit);
                        return;
                    }
                case "MainMenuButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.Menu);
                        return;
                    }
                case "SkipButton":
                    {
                        SceneControl.OnMenuSelection(SceneControl.SceneAction.PlayChapter);
                        return;
                    }
            }

    }
}
