using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] audioClips;
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    public float crossfadeDuration;
    public AudioSource OtherAudioSource => currentAudioSource == audioSourceA ? audioSourceB : audioSourceA;

    private AudioSource currentAudioSource;
    private string currentlyPlayingTrack;
    private float timeSpentPlayingCurrentTrack;
    private float currentMinimumPlayDuration;

    private void Awake()
    {
        // Are there any other game managers yet?
        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Sound Manager");
        }
        else
        {
            instance = this;
        }

        currentAudioSource = audioSourceA;
    }

    private void Update()
    {
        timeSpentPlayingCurrentTrack += Time.deltaTime;
    }

    internal static void PlayMusic(string trackName, float minimumPlayDuration = 0f)
    {
        instance.PlayMusicInternal(trackName, minimumPlayDuration);

    }

    private void PlayMusicInternal(string trackName, float minimumPlayDuration)
    {
        // If we're already playing this track or we haven't been playing the current track long enough
        if(currentlyPlayingTrack == trackName || timeSpentPlayingCurrentTrack < currentMinimumPlayDuration)
        {
            return;
        }

        // Find the audio clip for the track
        var trackClip = audioClips.FirstOrDefault(audioClip => audioClip.name == trackName);
        if (trackClip == null)
        {
            Debug.LogError($"Couldn't find a track called {trackName}");
            return;
        }

        // If the current audio source is playing something already
        if (currentAudioSource.isPlaying)
        {
            // Fade out the currently playing music
            currentAudioSource.DOFade(0f, crossfadeDuration);

            // Fade in the new music on the other source
            PlayMusicOnSource(OtherAudioSource, trackClip);

            // Swap the current music source
            currentAudioSource = OtherAudioSource;
        }
        // If nothing is playing yet
        else
        {
            // Play music on the current music source
            PlayMusicOnSource(currentAudioSource, trackClip);
        }

        currentlyPlayingTrack = trackName;
        currentMinimumPlayDuration = minimumPlayDuration;
        timeSpentPlayingCurrentTrack = 0f;

    }

    private void PlayMusicOnSource(AudioSource audioSource, AudioClip trackClip)
    {
        audioSource.clip = trackClip;
        audioSource.volume = 0f;
        audioSource.Play();
        audioSource.DOFade(1f, crossfadeDuration);
    }

    internal static void PlaySound(GameObject target, string soundName)
    {
        instance.PlaySoundInternal(target, soundName);
    }

    private void PlaySoundInternal(GameObject target, string soundName)
    {
        // Try to find an audio source on the target
        var audioSource = target.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an audio source
            audioSource = target.AddComponent<AudioSource>();
        }

        // Find the audio clip for the sound
        var soundClip = audioClips.FirstOrDefault(audioClip => audioClip.name == soundName);
        if (soundClip == null)
        {
            Debug.LogError($"Couldn't find a sound called {soundName}");
            return;
        }

        // Play the sound as a one-shot
        audioSource.PlayOneShot(soundClip);
    }
}
