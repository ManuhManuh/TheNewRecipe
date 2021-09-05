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

    private bool unloadPreviousScene;
    private bool activeOnLoad;
    private bool readyForPreload;
    private bool introFinished;
    private AsyncOperation asyncOperation;

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

    public bool ReadyForPreload
    {
        get
        {
            return readyForPreload;
        }
        set
        {
            readyForPreload = value;
        }
    }
    public bool IntroFinished
    {
        get
        {
            return introFinished;
        }
        set
        {
            introFinished = value;
        }
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
                introFinished = false;
                unloadPreviousScene = false;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene, activeOnLoad));

                return;

            case SceneAction.Menu:

                // Unload the current scene if it is not already the main menu
                // This the only scene that can be selected while it is the currently active scene (with controller menu button)

                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    unloadPreviousScene = true;
                    activeOnLoad = true;
                    StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene, activeOnLoad));
                }

                return;

            case SceneAction.Instructions:
                
                unloadPreviousScene = true;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("Instructions", unloadPreviousScene, activeOnLoad));

                return;

            case SceneAction.PlayIntro:

                // Load and play the intro scene
                unloadPreviousScene = true;  //previous scene is main menu
                activeOnLoad = true;
                StartCoroutine(ChangeScene("Intro", unloadPreviousScene, activeOnLoad));

                // set the new Game flag off, so the resume button will be available to the menu next time it is loaded
                newGame = false;

                return;

            case SceneAction.PlayChapter:

                // Note: this action will only happen if the scene is resumed after a pause; the initial playing is initiated following the intro finishing
                // TODO: make this work with multiple chapters (i.e. play next chapter)

                // Until there are more chapters, this doesn't need to do anything - it will eventually figure out which chapter is next and load it

                return;

            case SceneAction.Pause:

                unloadPreviousScene = false;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene, activeOnLoad));

                return;

            case SceneAction.Resume:
                // Unload the Main menu
                SceneManager.UnloadSceneAsync("MainMenu");

                // Activate the paused scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Chapter01"));
                
                return;

            case SceneAction.Credits:

                unloadPreviousScene = true;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("Credits", unloadPreviousScene, activeOnLoad));

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
        Image panelImage = panelToFade.GetComponent<Image>();
        Color newColour = panelImage.color;

        if (fadeOut)
        {
            // Fade the panel to black

            for (float i = 0; i <= fadeDuration; i += Time.deltaTime)
            {
                newColour.a = i;
                panelImage.color = newColour;

                yield return null;
            }

        }
        else
        {
            // Fade the panel to invisible

            for (float i = fadeDuration; i >= 0; i -= Time.deltaTime)
            {
                newColour.a = i;
                panelImage.color = newColour;

                yield return null;
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

        // Check if we are waiting for the intro to start
        if (readyForPreload == true && introFinished == false)
        {
            // Pause to make sure intro has started
            StartCoroutine(PreLoadChapter1());

            // Prevent this from being called again
            readyForPreload = false;

        }


        // Check if we have a scene on hold waiting for the intro to finish
        if (introFinished && asyncOperation.allowSceneActivation == false)
        {
            string sceneToUnload = "Intro";
            string sceneToActivate = "Chapter01";
            StartCoroutine(ActivateWaitingScene(sceneToUnload, sceneToActivate));
        }

    }

    private IEnumerator ChangeScene (string newScene, bool unloadOldScene, bool activeOnLoad)
    {
        Scene sceneToUnload = SceneManager.GetActiveScene();

        asyncOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        if (!activeOnLoad)
        {
            // Load but do not activate
            asyncOperation.allowSceneActivation = false;
        }
        else
        {
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            Scene sceneToActivate = SceneManager.GetSceneByName(newScene);
            SceneManager.SetActiveScene(sceneToActivate);
        }

        if (unloadPreviousScene)
        {
            // Note: this will not happen if the asyncOperation is paused - scene will be unloaded when new scene is activated
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        yield return null;

    }

    private IEnumerator ActivateWaitingScene(string sceneToUnload, string sceneToActivate)
    {
        Debug.Log("Starting ActivateWaitingScene");

        // allow the scene activation if we have a waiting scene
        if (asyncOperation.allowSceneActivation == false)
        {

            // finish load 
            asyncOperation.allowSceneActivation = true;
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

        }

        SceneManager.UnloadSceneAsync(sceneToUnload);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (SceneManager.GetSceneByName(sceneToActivate).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToActivate));
            Debug.Log($"{sceneToActivate} is the active scene!!");
        }
        else
        {
            Debug.Log($"{sceneToActivate} is not loaded");
        }

    }

    private IEnumerator PreLoadChapter1()
    {
        // wait for the menu to be unloaded 
        if (SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            yield return null;
        }

        // Pre-load the first chapter (but do not activate)
        unloadPreviousScene = false;
        activeOnLoad = false;
        StartCoroutine(ChangeScene("Chapter01", unloadPreviousScene, activeOnLoad));
    }
}
