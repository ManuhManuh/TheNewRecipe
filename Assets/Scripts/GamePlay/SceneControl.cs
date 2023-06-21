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
    private bool outroFinished;
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
        StayTuned,
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

    public bool OutroFinished
    {
        get
        {
            return outroFinished;
        }
        set
        {
            outroFinished = value;
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
            DontDestroyOnLoad(transform.root);
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
                outroFinished = false;
                unloadPreviousScene = false;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("Instructions", unloadPreviousScene, activeOnLoad));

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

                // Activate the paused scene, or load the first chapter to skip intro
                Scene firstChapter = SceneManager.GetSceneByName("Chapter01");
                if (firstChapter.isLoaded)
                {
                    SceneManager.SetActiveScene(firstChapter);
                }
                else
                {
                    SceneManager.LoadScene("Chapter01");

                }    
                
                return;

            case SceneAction.StayTuned:

                unloadPreviousScene = true;
                activeOnLoad = true;
                StartCoroutine(ChangeScene("Outro", unloadPreviousScene, activeOnLoad));

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

    private void Update()
    {

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

        // Check if the outro was playing and is now is finished

        if (outroFinished)
        {
            outroFinished = false;
            OnMenuSelection(SceneAction.Credits);
        }

    }

    public void AccessMainMenu()
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
    private IEnumerator ChangeScene (string newScene, bool unloadPreviousScene, bool activeOnLoad)
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

        if (unloadPreviousScene && sceneToUnload.name != "MasterScene")
        {
            // Note: this will not happen if the asyncOperation is paused - scene will be unloaded when new scene is activated
            SceneManager.UnloadSceneAsync(sceneToUnload);
 
        }

        yield return null;

    }

    private IEnumerator ActivateWaitingScene(string sceneToUnload, string sceneToActivate)
    {

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
