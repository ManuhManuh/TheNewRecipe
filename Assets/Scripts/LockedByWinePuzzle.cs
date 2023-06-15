using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedByWinePuzzle : LockableObject
{
    public override void OnUnlocked()
    {
        if (GameManager.winePuzzleSolved)
        {
            base.OnUnlocked();
        }
    }

}
