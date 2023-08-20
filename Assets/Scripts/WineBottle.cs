using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottle : MonoBehaviour
{
    [SerializeField] private List<AudioClip> glassClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> woodClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.CompareTag("WineBottle"))
        {
            // play a random glass clink sound
            audioSource.PlayOneShot(glassClips[Random.Range(0, glassClips.Count - 1)]);
        }
        else
        {
            // play a random wood clink sound
            audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Count - 1)]);
            
        }
    }

    private void OnBecameInvisible()
    {
        // check if it is actually blocked rather than just behind the player
    }

}
