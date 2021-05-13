using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public static bool wineBottlesPlaced;
    public static List<Color> ColourCycle = new List<Color>();

    private static bool kegsTapped;
    private static bool kegsCorrectlyColoured;
    private static int correctWineSlotsFilled;
    private Color baseColour;

    private void Awake()
    {
        // Are there any other game managers yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Game Manager");
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        baseColour = Color.black;
        // Collect the list of keg colours
        // Store the current keg colour, plus the collection of correct colours that will be cycled through
        var foundKegs = FindObjectsOfType<Keg>();
        foreach (var keg in foundKegs)
        {
            foreach (Transform child in keg.transform)
            {
                if (child.CompareTag("ColourRing"))
                {
                    // Store the currect colour ring colour if it hasn't already been stored
                    if (baseColour == Color.black)
                    {
                        baseColour = child.GetComponent<MeshRenderer>().material.color;
                        ColourCycle.Add(baseColour);
                    }
                    
                }
            }

            // Store the correct colour for the keg
            ColourCycle.Add(keg.GetComponent<Keg>().correctColour);
        }

        // Check the colour array
        /*
        for(int i = 0; i < ColourCycle.Count; i++)
        {
            Debug.Log($"Colour index {i}: {ColourCycle[i].ToString()}");
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        // If conditions necessary for win are complete
        if(kegsCorrectlyColoured && kegsTapped)
        {
            // Open the cask door
            // Play winning sequence
            Debug.Log("WINNER!!");
        }
    }

    internal static void OnWineBottlePlaced()
    {
        // Check to see if all slots have the correct fill status
        wineBottlesPlaced = FindObjectsOfType<WineBottle>().All(bottle => bottle.InACorrectSlot);
        if (wineBottlesPlaced)
        {
            Debug.Log("Wine puzzle solved");
        }
    }

    internal static void OnKegTapped()
    {
        // Check to see if all the kegs are tapped
        kegsTapped = FindObjectsOfType<Keg>().All(k => k.HasTap);

        // Nothing else happens in the game, but the condition is set for evaluating the win
    }

    internal static void OnKegColoured()
    {
        // Check if all kegs are correctly coloured
        kegsCorrectlyColoured = FindObjectsOfType<Keg>().All(k => k.ColourIsCorrect);
        if (kegsCorrectlyColoured)
        {
            Debug.Log("Colours correct - consider freezing them");
        }

    }

    internal static void OnWineBottleRemoved()
    {
        // Check to see if all slots have the correct fill status
        wineBottlesPlaced = FindObjectsOfType<WineBottle>().All(bottle => bottle.InACorrectSlot);

        // No need to check for solving because if a bottle is removed but not replaced the puzzle is not solved
    }
}
