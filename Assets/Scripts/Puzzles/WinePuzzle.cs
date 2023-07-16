using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WinePuzzle : MonoBehaviour, IPuzzle
{
   
    public string PuzzleName => puzzleName;
    public bool Solved => IsSolved();

    [SerializeField] private GameObject tap;
    [SerializeField] private XRGrabInteractable drawer;

    private string puzzleName = "WinePuzzle";
    private WineSlot[] wineSlots;

    private void Start()
    {
        wineSlots = FindObjectsOfType<WineSlot>();
    }

    public void CheckPuzzleStatus()
    {
        if (IsSolved())
        {
            OnSolved();
        }
    }

    public bool IsSolved()
    {
        bool solved = true;

        GameObject magicTapOfCheating = GameObject.Find("MagicEndingTap");
        if (magicTapOfCheating == null)
        {
            foreach (WineSlot slot in wineSlots)
            {
                if (!slot.HasCorrectFillStatus)
                {
                    solved = false;
                }
            }
        }

        return solved;
    }

    public void OnSolved()
    {
        // unlock the drawer 
        drawer.enabled = true;
        drawer.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        // enable the tap inside the drawer
        tap.SetActive(true);
    }
    
}
