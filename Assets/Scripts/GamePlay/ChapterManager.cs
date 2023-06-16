using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{

    [SerializeField] private List<IPuzzle> chapterPuzzles = new List<IPuzzle>();
    private GameManager gameManager;
    private WinSequence winSequence;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        winSequence = FindObjectOfType<WinSequence>();
    }

    public void CheckChapterStatus()
    {
        if (ChapterFinished())
        {
            OnChapterFinished();
        }
    }
    public bool ChapterFinished()
    {
        bool finished = true;

        foreach(IPuzzle puzzle in chapterPuzzles)
        {
            if (!puzzle.Solved)
            {
                finished = false;
            }
        }
        return finished;
    }

    public void OnChapterFinished()
    {
        winSequence.OnWin();
        
    }
    public void OnTransitionTeleportReached()
    {
        gameManager.AdvanceToNextChapter();
    }
}
