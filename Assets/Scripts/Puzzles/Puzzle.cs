using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzle
{
    string PuzzleName { get; }
    bool Solved { get; }
    bool IsSolved();
    void OnSolved();
    void CheckPuzzleStatus();
}
