using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneConductor : MonoBehaviour
{
    [SerializeField] GameObject player;
    public int CurrentScene => (int)currentSceneIndex;

    private SceneIndex currentSceneIndex;
    private SceneIndex preLoadingChapter;
    private SceneIndex waitingChapter;

    private bool chapterIsOpen;

    // Note: the SceneIndex enum needs to match the build index to work right
    // Chapters can be added at the end
    public enum SceneIndex
    {
        MasterScene,
        MainMenu,
        Instructions,
        Credits,
        Ending,
        Intro,
        Chapter01
    }

    private void Awake()
    {
        currentSceneIndex = SceneIndex.MasterScene;
        preLoadingChapter = SceneIndex.MasterScene; // condition when no chapter scenes are currently loading
        waitingChapter = SceneIndex.MasterScene;    // condition when no chapter scenes are awaiting activation
        chapterIsOpen = false;
    }

    private void Start()
    {
        SceneIndex sceneToLoad = SceneIndex.Instructions;

        bool unloadCurrentScene = false; // Only scene currently open should be Master, which should never be unloaded
        bool activateWhenLoaded = true;

        StartCoroutine(ChangeToNewScene(sceneToLoad, unloadCurrentScene, activateWhenLoaded));
    }


    /// <summary>
    /// Below are the methods that will be called to initiate scene changes
    /// </summary>

    public void ShowMainMenu()
    {
        bool activateWhenLoaded = true;

        // if current scene is a chapter don't unload it, just add the menu; otherwise unload it
        bool unloadCurrentScene = (int)currentSceneIndex < 6 && (int)currentSceneIndex > 0;

        StartCoroutine(ChangeToNewScene(SceneIndex.MainMenu, unloadCurrentScene, activateWhenLoaded));
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

        // chapters should be preloaded, as they are larger and take more time
        if (preLoadingChapter == SceneIndex.MasterScene)
        {
            // no chapter is currently being preloaded
            StartCoroutine(PreLoadScene(chapter));
        }

    }

    public void ActivateChapter(SceneIndex chapter)
    {
        // Activate a previously pre-loaded chapter

    }

    /// <summary>
    /// Below are the methods that actually load and unload the scenes
    /// </summary>

    private IEnumerator ChangeToNewScene(SceneIndex sceneIndexToLoad, bool unloadOldScene, bool activateWhenLoaded)
    {
        // Use this when previous scene needs to be unloaded (i.e., switching between UI scenes)
        // If the previous scene does NOT need to be unloaded, use AddNewScene instead

        // Make sure we never unload the Master scene
        if (currentSceneIndex == SceneIndex.MasterScene) yield return null;

        Scene sceneToUnload = SceneManager.GetSceneByBuildIndex((int)currentSceneIndex);
        Scene sceneToActivate = SceneManager.GetSceneByBuildIndex((int)sceneIndexToLoad);

        // load the new scene; always additive because the Master Scene can't be unloaded
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneIndexToLoad, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (unloadOldScene)
        {
            // when done, unload the old scene if required
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        if (activateWhenLoaded)
        {
            // activate the new scene and update currentSceneIndex
            SceneManager.SetActiveScene(sceneToActivate);
            currentSceneIndex = sceneIndexToLoad;

            // place the player for the scene
            PositionPlayerForScene();
        }

        yield return null;
    }

  
    private IEnumerator PreLoadScene(SceneIndex sceneToPreLoad)
    {

        preLoadingChapter = SceneIndex.MasterScene;
        waitingChapter = sceneToPreLoad;
        yield return null;
    }

    private void PositionPlayerForScene()
    {
        // find the start position game object for the current scene

    }

}
