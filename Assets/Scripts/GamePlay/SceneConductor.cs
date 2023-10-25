using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneConductor : MonoBehaviour
{
    //public static SceneConductor instance;

    //[SerializeField] GameObject player;
    //[SerializeField] List<Transform> startPositions = new List<Transform>();
    //// Note: above indexes need to match build indexes and SceneIndex enum

    //public int CurrentScene => (int)currentSceneIndex;
    //public bool ChapterIsOpen => chapterIsOpen;

    //private SceneIndex currentSceneIndex;
    //private SceneIndex waitingChapter;

    //private bool chapterIsOpen;
    //private AsyncOperation asyncOperation;
    //private Vector3 sceneStartPosition;
    //private Quaternion sceneStartRotation;
    //private Vector3 pausedPosition;
    //private Quaternion pausedRotation;
    //private GameObject levelObjects;


    //// Note: the SceneIndex enum needs to match the build index to work right
    //// Chapters can be added at the end
    //public enum SceneIndex
    //{
    //    MasterScene = 0,
    //    MainMenu = 1,
    //    Chapter01 = 2
    //}

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Debug.LogError("There was more than 1 Scene Control");
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(transform.root);
    //    }
       
    //    currentSceneIndex = SceneIndex.MasterScene;
    //    waitingChapter = SceneIndex.MasterScene;    // condition when no chapter scenes are awaiting activation
    //    chapterIsOpen = false;
    //    pausedPosition = gameObject.transform.position;
    //    pausedRotation = gameObject.transform.rotation;

    //}

    //private void Start()
    //{
    //    // load the main menu scene 

    //}
    ///// <summary>
    ///// Below are the methods that will be called to initiate scene changes
    ///// </summary>

    //public void ShowMainMenuInChapter()
    //{

    //    // from UI, use ShowNonChapterScene instead

    //    if (chapterIsOpen)    
    //    {
    //        levelObjects = GameObject.Find("LevelObjects");
    //        levelObjects.SetActive(false);
    //        pausedPosition = player.transform.position;
    //        pausedRotation = player.transform.rotation;
    //        bool activateWhenLoaded = true;
    //        bool unloadCurrentScene = false;
    //        StartCoroutine(ChangeToNewScene(SceneIndex.MainMenu, unloadCurrentScene, activateWhenLoaded));

    //    }

    //}

    //public void CloseMainMenu()
    //{
    //    if (chapterIsOpen)  // only time this should be used, as otherwise the menu is closed as byproduct of opening another scene
    //    {
    //        levelObjects.SetActive(true);
    //        // paused: just unload the main menu and put player back where they were
    //        StartCoroutine(UnloadScene(currentSceneIndex));
    //        PositionPlayerForScene(pausedPosition, pausedRotation);
    //    }

    //}

    //public void ShowNonChapterScene(SceneIndex sceneToShow)
    //{

    //    // Scenes that are not chapters and not the Master Scene are "NonChapter" scenes
    //    if ((int)sceneToShow > 5 || (int)sceneToShow == 0) return;

    //    bool activateWhenLoaded = true;
    //    bool unloadCurrentScene = true;
        
    //    StartCoroutine(ChangeToNewScene(sceneToShow, unloadCurrentScene, activateWhenLoaded));

    //}

  
    //public void PreLoadChapter(SceneIndex chapter)
    //{

    //    // make sure the scene is a chapter scene
    //    if ((int)chapter < 6) return;

    //    // chapters should be preloaded, as they are larger and take more time, but only one at a time
    //    if (asyncOperation.allowSceneActivation == true) 
    //    {
    //        // no chapter is currently being preloaded
    //        bool unloadOldScene = false;
    //        bool activateWhenLoaded = false;

    //        StartCoroutine(ChangeToNewScene(chapter, unloadOldScene, activateWhenLoaded));
    //    }

    //    waitingChapter = chapter;

    //}

    //public void ActivateChapter(SceneIndex chapter)
    //{

    //    // Activate a previously pre-loaded chapter
    //    StartCoroutine(ActivateWaitingChapter());
    //    chapterIsOpen = true;

    //}

    //public void ExitGame()
    //{
    //    Debug.Log("Game exited");
    //    // Exit the game
    //    Application.Quit();
    //}

    ///// <summary>
    ///// Below are the methods that actually load and unload the scenes
    ///// </summary>

    //private IEnumerator ChangeToNewScene(SceneIndex sceneIndexToLoad, bool unloadOldScene, bool activateWhenLoaded)
    //{
    //    if (SceneManager.GetSceneByBuildIndex((int)sceneIndexToLoad).isLoaded) yield break;
    //    // Make sure we never unload the Master scene
    //    if (currentSceneIndex == SceneIndex.MasterScene) unloadOldScene = false;

    //    Scene sceneToUnload = SceneManager.GetSceneByBuildIndex((int)currentSceneIndex);
    //    // Scene sceneToActivate = SceneManager.GetSceneByBuildIndex((int)sceneIndexToLoad);

    //    // load the new scene; always additive because the Master Scene can't be unloaded
    //    asyncOperation = SceneManager.LoadSceneAsync((int)sceneIndexToLoad, LoadSceneMode.Additive);
    //    asyncOperation.allowSceneActivation = activateWhenLoaded;

    //    while (!asyncOperation.isDone)
    //    {
    //        yield return null;
    //    }

    //    if (activateWhenLoaded)
    //    {
    //        // activate the new scene if required and update status variables
    //        // SceneManager.SetActiveScene(sceneToActivate);
    //        currentSceneIndex = sceneIndexToLoad;

    //        // place the player for the scene

    //        sceneStartPosition = startPositions[(int)currentSceneIndex].position;
    //        sceneStartRotation = startPositions[(int)currentSceneIndex].rotation;

    //        PositionPlayerForScene(sceneStartPosition, sceneStartRotation);
    //    }
    //    else
    //    {
    //        // flag as waiting
    //        waitingChapter = sceneIndexToLoad;
    //    }

    //    if (unloadOldScene)
    //    {
    //        // when loading is done, unload the old scene if required
    //        SceneManager.UnloadSceneAsync(sceneToUnload);

    //        // check if replacing chapter with non-chapter
    //        if ((int)sceneToUnload.buildIndex > 5 && (int) sceneIndexToLoad <= 5)
    //        {
    //            chapterIsOpen = false;
    //        }
    //    }

    //    yield return null;

    //}

    //private IEnumerator ActivateWaitingChapter()
    //{

    //    // allow the scene activation if we have a waiting chapter
    //    if (asyncOperation.allowSceneActivation == false)
    //    {
    //        // finish load 
    //        asyncOperation.allowSceneActivation = true;
    //        while (!asyncOperation.isDone)
    //        {
    //            yield return null;
    //        }

    //    }

    //    // position the player for the new scene
    //    sceneStartPosition = startPositions[(int)waitingChapter].position;
    //    sceneStartRotation = startPositions[(int)waitingChapter].rotation;
    //    PositionPlayerForScene(sceneStartPosition, sceneStartRotation);

    //    // unload the current scene
    //    SceneManager.UnloadSceneAsync((int)currentSceneIndex);
    //    while (!asyncOperation.isDone)
    //    {
    //        yield return null;
    //    }

    //    // update current scene and reset waiting
    //    currentSceneIndex = waitingChapter;
    //    waitingChapter = SceneIndex.MasterScene;    // default when no chapter is waiting

    //}

    //private IEnumerator UnloadScene(SceneIndex sceneToUnload)
    //{
    //    // Note: this is only needed for unloading the Main Menu when opened from a chapter

    //    SceneManager.UnloadSceneAsync((int)sceneToUnload);
    //    while (!asyncOperation.isDone)
    //    {
    //        yield return null;
    //    }

    //}

    //private void PositionPlayerForScene(Vector3 newPosition, Quaternion newRotation)
    //{
    //    player.transform.SetPositionAndRotation(newPosition, newRotation);
    //}

}
