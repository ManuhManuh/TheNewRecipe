using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keg : MonoBehaviour
{
    public Color correctColour;
    public Transform tapPlaceholder;
    public float maxDistanceDelta;
    public bool ColourIsCorrect => colourIsCorrect;
    public bool HasTap => hasTap;


    private bool colourIsCorrect;
    private bool hasTap;
    private Color currentColour;
    private int colourIndex;
    private bool placingTap;
    private Transform tap;
    private GameObject colourRing;

    private Color test;

    public void Start()
    {        
        colourIsCorrect = false;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ColourRing"))
            {
                colourRing = child.gameObject;
                test = colourRing.GetComponent <MeshRenderer>().material.color;
            }
        }
        colourIndex = 0;
    }

    private void Update()
    {
        if(placingTap)
        {
            tap.position = Vector3.MoveTowards(tap.position, tapPlaceholder.position, maxDistanceDelta * Time.deltaTime);
            if (tap.position == transform.position)
            {
                // Parent the bottle to the wine slot so it stays there
                tap.SetParent(tapPlaceholder);
                placingTap = false;

                // update status of slot to filled
                hasTap = true;

                // Let the Game Manager know that the slot was filled
                GameManager.OnKegTapped();
                Debug.Log($"Tap nicely set in {gameObject.name}");
            }
        }
    }

    public void OnChangeColour()
    {
        // Set the colour ring to the next colour in the cycle
        int newIndex = colourIndex < 5 ? colourIndex + 1 : 0;
        colourRing.GetComponent<MeshRenderer>().material.color = GameManager.ColourCycle[newIndex];
        currentColour = GameManager.ColourCycle[newIndex];

        // Reset the value of the current colour
        colourIndex = newIndex;

        // Check if the current colour is the correct colour

        if (currentColour == correctColour)
        {
            // Flag as having the correct colour
            colourIsCorrect = true;

            // Let the Game Manager know a keg reached the correct colour
            GameManager.OnKegColoured();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject hitMe = other.gameObject;

        // If the thing that collided was a tap
        if (hitMe.CompareTag("Tap"))
        {
            // Unparent it (from the grabber)
            hitMe.transform.SetParent(null);

            // Start the process of placing it 
            placingTap = true;

        }

        // If the thing that collided was the hammer
        if (hitMe.CompareTag("Hammer"))
        {
            // Change to the next colour in the cycle
            OnChangeColour();
        }
    }
}
