using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneConductor : MonoBehaviour
{
    public static SceneConductor instance;

    [SerializeField] GameObject player;
    public int CurrentScene => (int)currentSceneIndex;

    private SceneIndex currentSceneIndex;
    private SceneIndex waitingChapter;

    private bool chapterIsOpen;
    private AsyncOperation asyncOperation;
    private Transform sceneStartPosition;
    private Transform pausedPosition;

    // Note: the SceneIndex enum needs to match the build index to work right
    // Chapters can be added at the end
    public enum SceneIndex
    {
        MasterScene = 0,
        Instructions = 1,
        MainMenu = 2,
        Credits = 3,
        Ending = 4,
        Intro = 5,
        Chapter01 = 6
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There was more than 1 Scene Control");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.root);
        }

        currentSceneIndex = SceneIndex.MasterScene;
        waitingChapter = SceneIndex.MasterScene;    // condition when no chapter scenes are awaiting activation
        chapterIsOpen = false;
    }

    /// <summary>
    /// Below are the methods that will be called to initiate scene changes
    /// </summary>

    public void ShowMainMenuInChapter()
    {
        bool activateWhenLoaded = true;
        bool unloadCurrentScene;

        // from UI, use ShowNonChapterScene instead

        if (chapterIsOpen)    
        {
            // chapter: keep open, and record location to return to when pause is over
            unloadCurrentScene = false;
            pausedPosition = player.transform;
            StartCoroutine(ChangeToNewScene(SceneIndex.MainMenu, unloadCurrentScene, activateWhenLoaded));

        }
       
    }

    public void CloseMainMenu()
    {
        if (chapterIsOpen)  // only time this should be used, as otherwise the menu is closed as byproduct of opening another scene
        {
            // paused: just unload the main menu and put player back where they were
            StartCoroutine(UnloadScene(currentSceneIndex));
            PositionPlayerForScene(pausedPosition);

        }

    }

    public void ShowNonChapterScene(SceneIndex sceneToShow)
    {

        // Scenes that are not chapters and not the Master Scene are not NonChapter scenes
        if ((int)sceneToShow > 5 || (int)sceneToShow == 0) return;

        bool activateWhenLoaded = true;
        bool unloadCurrentScene = true;

        StartCoroutine(ChangeToNewScene(sceneToShow, unloadCurrentScene, activateWhenLoaded));

    }

  
    public void PreLoadChapter(SceneIndex chapter)
    {
        // make sure the scene is a chapter scene
        if ((int)chapter < 6) return;

        // chapters should be preloaded, as they are larger and take more time, but only one at a time
        if (asyncOperation.allowSceneActivation == true) 
        {
            // no chapter is currently being preloaded
            bool unloadOldScene = false;
            bool activateWhenLoaded = false;

            StartCoroutine(ChangeToNewScene(chapter, unloadOldScene, activateWhenLoaded));
        }

    }

    public void ActivateChapter(SceneIndex chapter)
    {
        // Activate a previously pre-loaded chapter
        StartCoroutine(ActivateWaitingChapter());
        chapterIsOpen = true;

    }

    public void ExitGame()
    {
        Debug.Log("Game exited");
        // Exit the game
        Application.Quit();
    }

    /// <summary>
    /// Below are the methods that actually load and unload the scenes
    /// </summary>

    private IEnumerator ChangeToNewScene(SceneIndex sceneIndexToLoad, bool unloadOldScene, bool activateWhenLoaded)
    {
        Debug.Log($"Attempting to load scene with build index {(int)sceneIndexToLoad}");
        Debug.Log($"Called with unload old = {unloadOldScene} and activateonload = {activateWhenLoaded}");

        // Make sure we never unload the Master scene
        if (currentSceneIndex == SceneIndex.MasterScene) unloadOldScene = false;

        Scene sceneToUnload = SceneManager.GetSceneByBuildIndex((int)currentSceneIndex);
        Scene sceneToActivate = SceneManager.GetSceneByBuildIndex((int)sceneIndexToLoad);

        // load the new scene; always additive because the Master Scene can't be unloaded
        asyncOperation = SceneManager.LoadSceneAsync((int)sceneIndexToLoad, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = activateWhenLoaded;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        Debug.Log($"Scene has been loaded?: {sceneToActivate.isLoaded}");

        if (activateWhenLoaded)
        {
            Debug.Log($"Activating {sceneToUnload.name}");
            Debug.Log($"Scene has been loaded?: {sceneToActivate.isLoaded}");

            // activate the new scene if required and update status variables
            SceneManager.SetActiveScene(sceneToActivate);
            currentSceneIndex = sceneIndexToLoad;

            // place the player for the scene
            sceneStartPosition = GameObject.FindGameObjectWithTag("StartPosition").transform;
            PositionPlayerForScene(sceneStartPosition);
   
        }
        else
        {
            // flag as waiting
            waitingChapter = sceneIndexToLoad;
        }

        if (unloadOldScene)
        {
            Debug.Log ($"Unloading {sceneToUnload.name}");

            // when loading is done, unload the old scene if required
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        yield return null;
    }

    private IEnumerator ActivateWaitingChapter()
    {
        // allow the scene activation if we have a waiting chapter
        if (asyncOperation.allowSceneActivation == false)
        {
            // finish load 
            asyncOperation.allowSceneActivation = true;
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

        }

        StartCoroutine(UnloadScene(currentSceneIndex));

        // Activate the new scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)waitingChapter));

        // position the player for the new scene
        sceneStartPosition = GameObject.FindGameObjectWithTag("StartPosition").transform;
        PositionPlayerForScene(sceneStartPosition);

        // update current scene and reset waiting
        currentSceneIndex = waitingChapter;
        waitingChapter = SceneIndex.MasterScene;    // default when no chapter is waiting
    }

    private IEnumerator UnloadScene(SceneIndex sceneToUnload)
    {
        // unload the current scene
        SceneManager.UnloadSceneAsync((int)sceneToUnload);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

    }

    private void PositionPlayerForScene(Transform newPosition)
    {
        player.transform.position = newPosition.position;
        player.transform.rotation = newPosition.rotation;
    }

}
