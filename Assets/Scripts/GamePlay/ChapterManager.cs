using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public Transform playerStartPosition;
    public GameObject levelContainer;

    [SerializeField] private List<IPuzzle> chapterPuzzles = new List<IPuzzle>();

    private GameManager gameManager;
    private WinSequence winSequence;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }
    private void Start()
    {
        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;

        gameManager = FindObjectOfType<GameManager>();
        winSequence = FindObjectOfType<WinSequence>();

        gameManager.CurrentChapter = this.GetComponent<ChapterManager>();

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
        
        foreach (IPuzzle puzzle in chapterPuzzles)
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

    public void OnAdvancementTriggered()
    {
        gameManager.AdvanceToNextChapter();
    }
    
}
