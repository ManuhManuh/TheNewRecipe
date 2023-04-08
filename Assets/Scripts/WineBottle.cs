using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottle : MonoBehaviour
{
    [SerializeField] private AudioClip[] wood;
    [SerializeField] private AudioClip[] glass;

    private Rigidbody bottleRigidbody;
    private AudioSource audioSource;
    private bool allowSounds;

    void Start()
    {
        bottleRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("DelaySoundEffects", 4);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (allowSounds)
        {
            string soundEffect = "";

            if (collision.gameObject.CompareTag("SolidSurface"))
            {
                // select wood sound
                AudioClip sound = SelectRandomClip("wood");
                soundEffect = sound.name;
            }

            if (collision.gameObject.CompareTag("WineBottle"))
            {
                //select glass sound
                AudioClip sound = SelectRandomClip("glass");
                soundEffect = sound.name;
            }

            if (soundEffect != "")
            {
                SoundManager.PlaySound(gameObject, soundEffect);
                Debug.Log($"Playing {soundEffect}");
            }
        }
           
    }

    private AudioClip SelectRandomClip(string soundType)
    {

        if(soundType == "wood")
        {
            return wood[Random.Range(0, wood.Length - 1)];
        }
        else
        {
            return wood[Random.Range(0, glass.Length - 1)];
        }
    }

    private IEnumerator DelaySoundEffects(int delay)
    {
        yield return new WaitForSeconds(delay);
        allowSounds = true;
    }

}
