using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private AudioClip hammerHit;
    [SerializeField] private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {

            // play hammer hit sound
            audioSource.PlayOneShot(hammerHit);

    }
}