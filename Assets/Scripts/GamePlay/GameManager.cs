using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ChapterManager CurrentChapter => currentChapter;

    [SerializeField] private List<string> chapters = new List<string>();
    private int currentChapterIndex = 0;    // Main menu is chapter 0, rest follow their natural number
    private ChapterManager currentChapter;

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

        if (currentChapterIndex >= chapters.Count)
        {
            // no more chapters: show the credits
            SceneControl.instance.GameOver();

        }
        else
        {
            // play the newly incremented chapter
            SceneControl.instance.OpenScene(chapters[currentChapterIndex]);
            currentChapter = null;
        }

    }

    private void Update()
    {
        if (currentChapter == null)
        {
            currentChapter = FindObjectOfType<ChapterManager>();
        }
    }

}
