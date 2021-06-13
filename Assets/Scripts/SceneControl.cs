using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public static SceneControl instance;
    public static bool newGame;
    
    public SceneAction currentSceneAction;
    public string mainMenuButton;

    private Scene currentScene;
    private bool unloadPreviousScene;

    public enum SceneAction
    {
        None,
        Menu,
        Instructions,
        PlayIntro,
        PlayChapter,
        Pause,
        Resume,
        Credits,
        Exit
    }
        
    private void Awake()
    {
        // Are there any other Scene Controls yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Scene Control");
        }
        else
        {
            instance = this;
        }

        
        
    }
    private void Start()
    {
        newGame = true;
        instance.OnMenuSelectionInternal(SceneAction.None);
        currentSceneAction = SceneAction.None;
    }

    internal static void OnMenuSelection(SceneAction newAction)
    {
        instance.OnMenuSelectionInternal(newAction);
    }

    private void OnMenuSelectionInternal(SceneAction newAction)
    {
        // Handle the different scene change scenarios
        // Note that the set up scene (containing singleton classes) is loaded first, and remains loaded throughout
        switch (newAction)
        {
            case SceneAction.None:
                // Should only happen at the very beginning of the game
 
                unloadPreviousScene = false;
                StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene));

                return;

            case SceneAction.Menu:

                // Unload the current scene if it is not already the main menu
                // This the only scene that can be selected while it is the currently active scene (with controller menu button)

                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    unloadPreviousScene = true;
                    StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene));
                }

                return;

            case SceneAction.Instructions:
                
                unloadPreviousScene = true;
                StartCoroutine(ChangeScene("Instructions", unloadPreviousScene));

                return;

            case SceneAction.PlayIntro:

                unloadPreviousScene = true;
                StartCoroutine(ChangeScene("Intro", unloadPreviousScene));

                return;

            case SceneAction.PlayChapter:
                // TODO: make this work with multiple chapters (i.e. play next chapter)

                // set the new Game flag off, so the resume button will be available to the menu next time it is loaded
                newGame = false;

                unloadPreviousScene = true;
                StartCoroutine(ChangeScene("Chapter01", unloadPreviousScene));

                // Start the ambient game music
                SoundManager.PlayMusic("Alex Mason - Prisoner");

                return;

            case SceneAction.Pause:

                unloadPreviousScene = false;
                StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene));

                return;

            case SceneAction.Resume:
                // Unload the Main menu
                SceneManager.UnloadSceneAsync("MainMenu");

                // Activate the paused scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Chapter01"));
                
                return;

            case SceneAction.Credits:

                unloadPreviousScene = true;
                StartCoroutine(ChangeScene("Credits", unloadPreviousScene));

                return;

            case SceneAction.Exit:
                Debug.Log("Game exited");
                // Exit the game
                Application.Quit();

                return;
            
        }

        currentSceneAction = newAction;
    }

    internal static void OnPanelFadeRequest (GameObject panelToFade, float fadeDuration, SceneAction nextSceneAction, bool fadeOut)
    {
        instance.OnPanelFadeRequestInternal(panelToFade, fadeDuration, nextSceneAction, fadeOut);
    }

    private void OnPanelFadeRequestInternal(GameObject panelToFade, float fadeDuration, SceneAction nextSceneAction, bool fadeOut)
    {
        StartCoroutine(FadePanel(panelToFade, fadeDuration, nextSceneAction, fadeOut));
    }

    private IEnumerator FadePanel(GameObject panelToFade, float fadeDuration, SceneAction nextSceneAction, bool fadeOut)
    {
        float fadeTimer = 0;
        Image panelImage = panelToFade.GetComponent<Image>();
        Color newColour = panelImage.GetComponent<Color>();

        if (fadeOut)
        {
            // Fade the panel to black
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                newColour.a = 1.0f - Mathf.Clamp01(fadeTimer / fadeDuration);
                panelImage.color = newColour;
            }
            
        }
        else
        {
            // Fade the panel to invisible
            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                newColour.a = 0f + Mathf.Clamp01(fadeTimer / fadeDuration);
                panelImage.color = newColour;
            }
        }

        // Let panel stay for three seconds
        yield return new WaitForSeconds(3f);

        // Play next scene
        OnMenuSelectionInternal(nextSceneAction);

        yield return null;
    }
    private void Update()
    {
        // Check to see if the player wants to access the Main Menu
        if (Input.GetButtonDown(mainMenuButton))
        {
            
            if (currentSceneAction == SceneAction.PlayChapter)
            {
                // Pause the game, which will display the main menu
                OnMenuSelection(SceneAction.Pause);
            }
            else
            {
                // Unload the current scene (leaving the main menu only)
                OnMenuSelection(SceneAction.Menu);
            }
            
        }

    }

    private IEnumerator ChangeScene (string newScene, bool unloadOldScene)
    {
        // Unload previous scene if required
        if (unloadOldScene)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        // Load new scene
        SceneManager.LoadScene(newScene, LoadSceneMode.Additive);

        // Wait until next frame
        yield return null;

        // Set new scene active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

    }



}
