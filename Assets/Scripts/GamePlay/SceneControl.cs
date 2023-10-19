using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public static SceneControl instance;

    private GameObject menuContainer;
    private Vector3 menuPlayerPosition;
    private AsyncOperation asyncOperation;
    private string currentChapter;
    private string previousChapter;
    private GameObject player;
    private Vector3 lastPlayerPosition;

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
        yield return new WaitForSeconds(1.0f);

        menuContainer = GameObject.Find("MainMenu");
        menuPlayerPosition = GameObject.Find("PlayerStartPosition").transform.position;
    }
    public void OpenScene(string sceneName)
    {
        // all chapters will have level objects in a container which will start out deactivated (unless this lags)
        // all chapters will open async and be activated intentionally (not just when done)
        
        if (currentChapter != null) previousChapter = currentChapter;
        currentChapter = sceneName;

        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        if (previousChapter != null) SceneManager.UnloadSceneAsync(sceneName);

    }

    public void ActivateLoadedScene()
    {
        if (asyncOperation.allowSceneActivation == false)
        {
            asyncOperation.allowSceneActivation = true;
            player.transform.position = GameManager.instance.CurrentChapter.playerStartPosition.position;

        }

    }

    public void ShowMainMenu(string sceneToUnload)
    {
     
        if(sceneToUnload == "None")
        {
            PauseChapter();

        }
        else
        {
            ReturnToMainWithUnload(sceneToUnload);

        }
        
    }

    private void PauseChapter()
    {
        // hide the chapter and unhide the menu
        lastPlayerPosition = player.transform.position;
        GameManager.instance.CurrentChapter.levelContainer.SetActive(false);
        menuContainer.SetActive(true);
        player.transform.position = menuPlayerPosition;

    }

    public void ResumeChapter()
    {
        // hide the menu and unhide the chapter 
        menuContainer.SetActive(false);
        player.transform.position = lastPlayerPosition;
        GameManager.instance.CurrentChapter.levelContainer.SetActive(true);

    }

    public void GameOver()
    {
        ReturnToMainWithUnload(GameManager.instance.CurrentChapter.name);
    }

    public void ReturnToMainWithUnload(string sceneToUnload)
    {
        // basically only happens with placeholder UI, eventually with Game Over to allow restart/credits, etc.

        if (sceneToUnload != "None")
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
            menuContainer.SetActive(true);
        }

    }

}
