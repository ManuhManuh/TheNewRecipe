using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegPuzzle : MonoBehaviour, IPuzzle
{
    public string PuzzleName => puzzleName;
    public bool Solved => IsSolved();
    public List<Color> ColourCycle = new List<Color>();

    private string puzzleName = "KegPuzzle";
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

        GameObject magicTapOfCheating = GameObject.Find("MagicEndingTap");
        if (magicTapOfCheating == null)
        {
            foreach (Keg keg in kegs)
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
        }

        return solved;
    }

    public void OnSolved()
    {
        chapterManager.CheckChapterStatus();
    }
    
}
