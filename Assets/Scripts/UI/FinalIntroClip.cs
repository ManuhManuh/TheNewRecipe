using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalIntroClip : MonoBehaviour
{

    public AudioClip finalClip;
    public GameObject finalClipSource;

    void Start()
    {
        // Play final clip
        SoundManager.PlaySound(finalClipSource, finalClip.name);
    }

 
}
