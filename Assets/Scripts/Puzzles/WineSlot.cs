using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineSlot : MonoBehaviour
{
    public bool CorrectFillStatus => correctFillStatus;

    [SerializeField] private bool shouldBeFilled;
    private bool correctFillStatus;
    private WinePuzzle winePuzzle;

    private void Start()
    {
        winePuzzle = FindObjectOfType<WinePuzzle>();

        // Check if this slot should be filled 
        shouldBeFilled = gameObject.CompareTag("CorrectWineSlot");

        // Initialize the fill status
        correctFillStatus = !shouldBeFilled;

    }

    public void WineBottlePlaced()
    {
        // Update the correctly filled status
        correctFillStatus = shouldBeFilled;

        // Get the puzzle status updated
        winePuzzle.CheckPuzzleStatus();

    }

    public void WineBottleRemoved()
    {
        // Update the correctly filled status
        correctFillStatus = shouldBeFilled;

        // Get the puzzle status updated
        winePuzzle.CheckPuzzleStatus();

    }
}
