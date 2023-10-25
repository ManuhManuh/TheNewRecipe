using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public static SceneControl instance;
    public bool ChapterLoaded => chapterLoaded;

    private GameObject menuContainer;
    private Menu menu;
    private Vector3 menuPlayerPosition;
    private Quaternion menuPlayerRotation;
    private AsyncOperation asyncOperation;
    private string currentChapter;
    private string previousChapter;
    private GameObject player;
    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;
    private bool chapterLoaded;

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
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(InitializeMenu());

    }

    private IEnumerator InitializeMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

        // allow time for MainMenu to load 
        //while (SceneManager.GetActiveScene().ToString() !="MainMenu")
        while(!SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            yield return null;
        }

        menuContainer = GameObject.Find("MainMenu"); // contains all objects in the menu scene - used for activation/deactivation
        menu = FindObjectOfType<Menu>();    // has the Menu script on it
        GameObject startMarker = GameObject.Find("PlayerStartPosition");
        menuPlayerPosition = startMarker.transform.position;
        menuPlayerRotation = startMarker.transform.rotation;

        GameManager.instance.AdvanceToNextChapter();

    }
    public IEnumerator OpenScene(string sceneName)
    {
        // all chapters will have level objects in a container which will start out deactivated (unless this lags)
        // all chapters will open async and be activated intentionally (not just when done)
        chapterLoaded = false;

        if (currentChapter != null) previousChapter = currentChapter;
        currentChapter = sceneName;

        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        while(asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        chapterLoaded = true;

        if (previousChapter != null) SceneManager.UnloadSceneAsync(sceneName);


    }

   
    public void ActivateLoadedScene()
    {
        if (asyncOperation.allowSceneActivation == false)
        {
            asyncOperation.allowSceneActivation = true;
            
        }

    }

    public void ShowMainMenu()
    {
     
        if(chapterLoaded)
        {
            PauseChapter();
        }
        else
        {
            menu.ReturnToMain();
        }
        
    }

    private void PauseChapter()
    {
        // hide the chapter and unhide the menu

        lastPlayerPosition = player.transform.position;
        lastPlayerRotation = player.transform.rotation;

        Debug.Log($"Deactivating {GameManager.instance.CurrentChapter.levelContainer.name}");

        GameManager.instance.CurrentChapter.levelContainer.SetActive(false);
        menuContainer.SetActive(true);
        menu.ShowPauseMenu();

        player.transform.position = menuPlayerPosition;
        player.transform.rotation = menuPlayerRotation;

    }

    public void ResumeChapter()
    {
        // hide the menu and unhide the chapter 
        ActivateLoadedScene();  // used if we going from the menu to a newly loaded chapter

        menuContainer.SetActive(false);
        player.transform.position = lastPlayerPosition;
        player.transform.rotation = lastPlayerRotation;

        GameManager.instance.CurrentChapter.levelContainer.SetActive(true);

    }

    public void GameOver()
    {
        SceneManager.UnloadSceneAsync(currentChapter);
        menuContainer.SetActive(true);
    }

}
