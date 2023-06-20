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

    private int currentChapterIndex = 0;
    
    public void AdvanceToNextChapter()
    {
        currentChapterIndex++;
        // SceneControl.SceneAction nextScene = chapters[currentChapterIndex].name; // use this when there are more chapters to test with

        SceneControl.OnMenuSelection(SceneControl.SceneAction.StayTuned);

    }

    
}
