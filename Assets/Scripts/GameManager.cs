using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool sitting = true;
    public static bool wineBottlesPlaced;
    public static List<Color> ColourCycle = new List<Color>();
    public static bool allPuzzlesSolved;

    private static bool kegsTapped;
    private static bool kegsCorrectlyColoured;
    private static Color baseColour;
    private static LockedByWinePuzzle hiddenDrawer;

    private bool chapterIsLoaded;

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
        chapterIsLoaded = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!chapterIsLoaded && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Chapter01"))
        {
            // This is the first frame after chapter is loaded
            OnChapterLoaded();
            chapterIsLoaded = true;
        }
        else
        {
            if (chapterIsLoaded && kegsCorrectlyColoured && kegsTapped)
            {
                allPuzzlesSolved = true;

                // Play end of game music
                SoundManager.PlayMusic("Alex Mason - Watchword");
            }
        }

    }

    internal static void OnChapterLoaded()
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

        // Find the drawer that is locked by the wine puzzle
        hiddenDrawer = FindObjectOfType<LockedByWinePuzzle>();
    }
    internal static void OnWineSlotUpdated()
    {
        // Check to see if all slots have the correct fill status
        wineBottlesPlaced = FindObjectsOfType<WineSlot>().All(slot => slot.CorrectFillStatus);

        if (wineBottlesPlaced)
        {
            hiddenDrawer.GetComponent<GrabbableObject>().ObjectLocked = false;
            hiddenDrawer.handle.SetActive(true);

            // Debug.Log("Wine bottles correct - consider freezing the bottles");
        }

    }


    internal static void OnKegTapped()
    {
        // Check to see if all the kegs are tapped
        kegsTapped = FindObjectsOfType<TapSensor>().All(sensor => sensor.HasTap);

        // Nothing else happens in the game, but the condition is set for evaluating the win
        // Debug.Log("Kegs all tapped");
    }

    internal static void OnKegColoured()
    {
        // Check if all kegs are correctly coloured
        kegsCorrectlyColoured = FindObjectsOfType<Keg>().All(k => k.ColourIsCorrect);
        if (kegsCorrectlyColoured)
        {
            // Debug.Log("Colours correct - consider freezing the colours");
        }

    }

    internal static void OnFinalTeleport()
    {
        // This may have additional functionality (and a chapter parameter) if/when there are more chapters
        SceneControl.OnMenuSelection(SceneControl.SceneAction.StayTuned);

    }

    
}
