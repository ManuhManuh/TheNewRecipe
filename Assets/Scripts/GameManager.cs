using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool sitting = true;
    public static bool wineBottlesPlaced;
    public static List<Color> ColourCycle = new List<Color>();
    public static bool allPuzzlesSolved;
    
    private static bool kegsTapped;
    private static bool kegsCorrectlyColoured;
    private Color baseColour;
    private static LockedByWinePuzzle hiddenDrawer;

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
        allPuzzlesSolved = false;

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

        // Find the drawer that is locked by the wine puzzle
        hiddenDrawer = FindObjectOfType<LockedByWinePuzzle>();

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.PlayMusic("Alex Mason - Prisoner");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.PlayMusic("Alex Mason - Watchword");
        }

        // If conditions necessary for win are complete
        if (kegsCorrectlyColoured && kegsTapped)
        {
            allPuzzlesSolved = true;

            // Play end of game music
            SoundManager.PlayMusic("Alex Mason - Watchword");

            Debug.Log("WINNER!!!!");

            // TODO: Game winning animation or whatever visual needed

            
        }
        else
        {
            SoundManager.PlayMusic("Alex Mason - Prisoner");
        }
    }

    internal static void OnWineSlotUpdated()
    {
        // Check to see if all slots have the correct fill status
        wineBottlesPlaced = FindObjectsOfType<WineSlot>().All(slot => slot.CorrectFillStatus);

        if (wineBottlesPlaced)
        {
            hiddenDrawer.GetComponent<GrabbableObject>().ObjectLocked = false;
            hiddenDrawer.handle.SetActive(true);

            Debug.Log("Wine bottles correct - consider freezing the bottles");
        }
        
    }


    internal static void OnKegTapped()
    {
        // Check to see if all the kegs are tapped
        kegsTapped = FindObjectsOfType<TapSensor>().All(sensor => sensor.HasTap);

        // Nothing else happens in the game, but the condition is set for evaluating the win
        Debug.Log("Kegs all tapped");
    }

    internal static void OnKegColoured()
    {
        // Check if all kegs are correctly coloured
        kegsCorrectlyColoured = FindObjectsOfType<Keg>().All(k => k.ColourIsCorrect);
        if (kegsCorrectlyColoured)
        {
            Debug.Log("Colours correct - consider freezing the colours");
        }

    }

    
}
