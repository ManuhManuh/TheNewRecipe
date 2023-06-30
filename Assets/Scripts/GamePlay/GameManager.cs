using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    [SerializeField] private List<ChapterManager> chapters = new List<ChapterManager>();

    private int currentChapterIndex = 0;

    private void Awake()
    {
        // Are there any other game managers yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Game Manager");
        }
        else
        {
            instance = this;
        }
    }

    public void AdvanceToNextChapter()
    {
        currentChapterIndex++;
        
        // use this until there are more chapters and the chapter flow mechanism is built; then, remove this
        SceneControl.OnMenuSelection(SceneControl.SceneAction.StayTuned);

        // SceneControl.SceneAction nextScene = chapters[currentChapterIndex].name; // use this when there are more chapters to test with

        // if the chapter specifies where the player should start, put the player there
        //if (chapters[currentChapterIndex].playerStartPosition != null)
        //{
        //    player.transform.position = chapters[currentChapterIndex].playerStartPosition.position;
        //}

    }

    
}
