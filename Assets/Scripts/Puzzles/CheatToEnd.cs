using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatToEnd : MonoBehaviour
{
    public WinSequence winSequence;


    private void Start()
    {
        winSequence = FindObjectOfType<WinSequence>();
    }
    public void SkipToEnd()
    {
        winSequence.OnWin();
    }
    

}
