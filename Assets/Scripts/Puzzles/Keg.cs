using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keg : MonoBehaviour
{
    public bool ColourIsCorrect => colourIsCorrect;
    public Color CorrectColour => correctColour;
    public bool HasTap
    {
        get
        {
            return hasTap;
        }
        set
        {
            hasTap = value;
        }
    }

    [SerializeField] private Color correctColour;

    private bool colourIsCorrect;
    private bool hasTap;
    private Color currentColour;
    private int colourIndex;
    private GameObject colourRing;
    private KegPuzzle kegPuzzle;

    public void Start()
    {
        kegPuzzle = FindObjectOfType<KegPuzzle>();
        colourIsCorrect = false;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ColourRing"))
            {
                colourRing = child.gameObject;
            }
        }
        colourIndex = 0;
    }

    public void OnChangeColour()
    {
        // Set the colour ring to the next colour in the cycle
        int newIndex = colourIndex < 5 ? colourIndex + 1 : 0;
        colourRing.GetComponent<MeshRenderer>().material.color = kegPuzzle.ColourCycle[newIndex];
        currentColour = kegPuzzle.ColourCycle[newIndex];

        // Reset the value of the current colour
        colourIndex = newIndex;

        // Check if the current colour is the correct colour

        if (currentColour == correctColour)
        {
            // Flag as having the correct colour
            colourIsCorrect = true;

        }

        // Get the puzzle status updated
        kegPuzzle.CheckPuzzleStatus();
}

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitMe = other.gameObject;

        // Play the hitting sound
        SoundManager.PlaySound(gameObject, "HittingCask");

        // If the thing that collided was the hammer
        if (hitMe.CompareTag("Hammer"))
        {
            // Change to the next colour in the cycle
            OnChangeColour();
        }
    }

    public void TapPlaced()
    {
        hasTap = true;
        kegPuzzle.CheckPuzzleStatus();
    }

    public void TapRemoved()
    {
        hasTap = false;
        kegPuzzle.CheckPuzzleStatus();
    }
    public void OnGameWon() 
    {
        // Turn ring glowing white
        colourRing.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
