using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineSlot : MonoBehaviour
{
    public bool HasCorrectFillStatus => fillStatusIsCorrect;

    [SerializeField] private bool shouldBeFilled;
    private bool fillStatusIsCorrect;
    private WinePuzzle winePuzzle;

    private void Start()
    {
        winePuzzle = FindObjectOfType<WinePuzzle>();

        // Initialize the fill status (all are empty, so is correct if it should not be filled)
        fillStatusIsCorrect = !shouldBeFilled;

    }

    public void WineBottlePlaced()
    {
        // Update the correctly filled status
        fillStatusIsCorrect = shouldBeFilled;

        // Get the puzzle status updated
        winePuzzle.CheckPuzzleStatus();

    }

    public void WineBottleRemoved()
    {
        // Update the correctly filled status
        fillStatusIsCorrect = !shouldBeFilled;

        // Get the puzzle status updated
        winePuzzle.CheckPuzzleStatus();

    }
}
