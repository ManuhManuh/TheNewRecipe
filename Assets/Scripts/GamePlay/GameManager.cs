﻿using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    private int currentChapterIndex = 6;    // this is Chapter 1

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

    private void Start()
    {
       SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Instructions);
    }

    public void AdvanceToNextChapter()
    {
        currentChapterIndex++;

        if (currentChapterIndex >= Enum.GetNames(typeof(SceneConductor.SceneIndex)).Length)
        {
            // no more chapters: play the ending
            SceneConductor.instance.ShowNonChapterScene(SceneConductor.SceneIndex.Ending);
        }
        else
        {
            // play the newly incremented chapter
            SceneConductor.instance.ActivateChapter((SceneConductor.SceneIndex)currentChapterIndex);
        }

    }

    
}
