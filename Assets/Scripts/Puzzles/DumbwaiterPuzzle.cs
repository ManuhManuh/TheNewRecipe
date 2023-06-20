using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbwaiterPuzzle : MonoBehaviour, IPuzzle
{
    public string PuzzleName => puzzleName;
    public bool Solved => IsSolved();

    [SerializeField] private GameObject hiddenTap;
    [SerializeField] private GameObject interiorTeleportTarget;

    private string puzzleName = "DumbwaiterPuzzle";
    private DumbwaiterDoor dumbwaiterDoor;

    void Start()
    {
        dumbwaiterDoor = FindObjectOfType<DumbwaiterDoor>();
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
        return dumbwaiterDoor.Frozen;
    }

    public void OnSolved()
    {
        // enable the tap inside the dumbwaiter
        hiddenTap.SetActive(true);

        // Enable the teleport target inside the dumbwaiter
        interiorTeleportTarget.SetActive(true);
    }


}
