﻿using System.Collections;
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

    // Note: replace this with a Chapter property when there are more chapters
    [SerializeField] private List<Transform> startPositions = new List<Transform>();
    [SerializeField] private GameObject player;

    private bool unloadPreviousScene;
    private bool activeOnLoad;
    private bool readyForPreload;
    private bool introFinished;
    private bool outroFinished;
    private AsyncOperation asyncOperation;
    private SceneAction previousSceneAction;
    private Transform pausedPlayerPosition;
    private string currentChapter;
    private bool menuIsOpen;

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

        previousSceneAction = SceneAction.None;
        menuIsOpen = false;
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
        previousSceneAction = currentSceneAction;
        currentSceneAction = newAction;

        switch (newAction)
        {
            case SceneAction.None:
                // Should only happen at the very beginning of the game
                introFinished = false;
                outroFinished = false;
                unloadPreviousScene = false;
                activeOnLoad = true;
                Debug.Log("Starting coroutine for loading Instructions");
                StartCoroutine(ChangeScene("Instructions", unloadPreviousScene, activeOnLoad));

                return;

            case SceneAction.Menu:

                // Unload the current scene if it is not already the main menu
                // This the only scene that can be selected while it is the currently active scene (with controller menu button)

                if (previousSceneAction == SceneAction.PlayChapter)
                {
                    OnMenuSelection(SceneAction.Pause);
                }
                else
                {
                    // navigation between UI scenes
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                    {
                        unloadPreviousScene = true;
                        activeOnLoad = true;
                    }

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

                // record current chapter to reload when resuming
                Debug.Log("Play chapter called");
                currentChapter = FindObjectOfType<ChapterManager>().name;

                // Until there are more chapters, this doesn't need to do anything else - it will eventually figure out which chapter is next and load it

                return;

            case SceneAction.Pause:
                // record player position to return to when resuming
                pausedPlayerPosition = player.transform;
  
                // load the menu 
                unloadPreviousScene = true;    // trying it with unload 
                activeOnLoad = true;
                StartCoroutine(ChangeScene("MainMenu", unloadPreviousScene, activeOnLoad));

                return;

            case SceneAction.Resume:

                unloadPreviousScene = true;    
                activeOnLoad = true;

                StartCoroutine(ChangeScene(currentChapter, unloadPreviousScene, activeOnLoad));

                // reset the player position
                PositionPlayerForScene();
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
            currentSceneAction = SceneAction.PlayChapter;

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
        // do nothing if trying to open menu and it's already there
        if (newScene == "MainMenu" && menuIsOpen)
        {
            Debug.Log("Tried to open MainMenu when it was already open");
            yield break;
        }

        Scene sceneToUnload = SceneManager.GetActiveScene();
        if (sceneToUnload.name == "MainMenu") menuIsOpen = false;

        asyncOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        if (newScene == "MainMenu") menuIsOpen = true;

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
            PositionPlayerForScene();
        }

        if (unloadPreviousScene && sceneToUnload.name != "MasterScene")
        {
            Debug.Log($"Unloading {sceneToUnload.name}");
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
            PositionPlayerForScene();
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

    private void PositionPlayerForScene()
    {
        Transform newPosition;

        if (currentSceneAction == SceneAction.Resume)
        {
            newPosition = pausedPlayerPosition;
        }
        else
        {
            newPosition = startPositions[(int)currentSceneAction];
        }
        
        player.transform.position = newPosition.position;
        player.transform.rotation = newPosition.rotation;
    }
}
