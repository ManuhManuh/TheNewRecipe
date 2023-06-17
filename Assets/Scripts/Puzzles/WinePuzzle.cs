using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinePuzzle : MonoBehaviour, IPuzzle
{
    [SerializeField] private string puzzleName;
    
    public string PuzzleName => puzzleName;
    public bool Solved => IsSolved();

    private WineSlot[] wineSlots;
    private ChapterManager chapterManager;

    private void Start()
    {
        chapterManager = FindObjectOfType<ChapterManager>();
        wineSlots = FindObjectsOfType<WineSlot>();
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

        foreach (WineSlot slot in wineSlots)
        {
            if (!slot.CorrectFillStatus)
            {
                solved = false;
            }
        }
        return solved;
    }

    public void OnSolved()
    {
        
        chapterManager.CheckChapterStatus();
    }
    
}
