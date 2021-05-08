using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keg : MonoBehaviour
{
    public Color correctColour;
    public Transform tapPlaceholder;
    public float maxDistanceDelta;

    private bool colourIsCorrect;
    private Color currentColour;
    private bool hasTap;
    private bool placingTap;
    private Transform tap;

    private List<Color> colourCycle => new List<Color>();
    public bool ColourIsCorrect => colourIsCorrect;
    public bool HasTap => hasTap;

    public void Start()
    {
        colourCycle.Add(Color.blue);
        colourCycle.Add(Color.magenta);
        colourCycle.Add(Color.red);
        colourCycle.Add(Color.green);
        colourCycle.Add(Color.yellow);
        colourCycle.Add(Color.black);

        currentColour = Color.black;
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
        // Find the current colour
        int indexOfCurrentColour = colourCycle.IndexOf(currentColour);

        // Find which colour is next in the list
        int nextColourIndex = (indexOfCurrentColour > colourCycle.Count ? 0 : indexOfCurrentColour + 1);

        // Set the object to that colour
        gameObject.GetComponent<Material>().color = colourCycle[nextColourIndex];

        // Reset the value of the current colour
        currentColour = colourCycle[nextColourIndex];
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject hitMe = collision.gameObject;
        // If the thing that collided was a tap
        if (hitMe.CompareTag("Tap"))
        {
            // Unparent it (from the grabber)
            hitMe.transform.SetParent(null);

            // Start the process of placing it 
            placingTap = true;

        }

        // If the thing that collided was the hammer
        if(hitMe.name == "Hammer")
        {
            // Change to the next colour in the cycle
            OnChangeColour();
        }
        
    }


}
