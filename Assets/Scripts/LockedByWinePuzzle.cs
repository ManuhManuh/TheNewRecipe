using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByWinePuzzle : LockableObject
{
    private WinePuzzle winePuzzle;
    public override void OnUnlocked()
    {
        if (winePuzzle.Solved)
        {
            base.OnUnlocked();
        }
    }

}
