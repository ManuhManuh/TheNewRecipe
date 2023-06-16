using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegPuzzle : MonoBehaviour, IPuzzle
{
    public string PuzzleName => throw new System.NotImplementedException();
    public bool Solved => throw new System.NotImplementedException();
    public List<Color> ColourCycle = new List<Color>();

    [SerializeField] private string puzzleName;

    private Keg[] kegs;
    private ChapterManager chapterManager;

    private void Start()
    {
        chapterManager = FindObjectOfType<ChapterManager>();
        kegs = FindObjectsOfType<Keg>();

        foreach (var keg in kegs)
        {
            // Store the correct colour for the keg - use to cycle through colors
            ColourCycle.Add(keg.GetComponent<Keg>().CorrectColour);
        }
    }

    public void CheckPuzzleStatus()
    {
        if (IsSolved())
        {
            OnSolved();
        }
    }
    public bool IsSolved()
    {
        bool solved = true;

        foreach(Keg keg in kegs)
        {
            if (!keg.HasTap)
            {
                solved = false;
            }
            else
            {
                if (!keg.ColourIsCorrect)
                {
                    solved = false;
                }
            }
        }

        return solved;
    }

    public void OnSolved()
    {
        chapterManager.CheckChapterStatus();
    }
    
}
