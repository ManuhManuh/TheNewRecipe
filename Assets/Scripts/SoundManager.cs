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

    internal static void PlayMusic(string trackName, float minimumPlayDuration = 0f, float volumeLevel = 1f)
    {
        instance.PlayMusicInternal(trackName, minimumPlayDuration, volumeLevel);

    }

    private void PlayMusicInternal(string trackName, float minimumPlayDuration, float volumeLevel)
    {
        // If we're already playing this track or we haven't been playing the current track long enough
        if(currentlyPlayingTrack == trackName || timeSpentPlayingCurrentTrack < currentMinimumPlayDuration)
        {
            // Adjust volume on current track
            currentAudioSource.volume = volumeLevel;

            return;
        }

        // Find the audio clip for the track
        var trackClip = audioClips.FirstOrDefault(audioClip => audioClip.name == trackName);
        if (trackClip == null)
        {
            return;
        }

        // If the current audio source is playing something already
        if (currentAudioSource.isPlaying)
        {
            // Fade out the currently playing music
            currentAudioSource.DOFade(0f, crossfadeDuration);

            // Fade in the new music on the other source
            PlayMusicOnSource(OtherAudioSource, trackClip, volumeLevel);

            // Swap the current music source
            currentAudioSource = OtherAudioSource;

        }
        // If nothing is playing yet
        else
        {
            // Play music on the current music source
            PlayMusicOnSource(currentAudioSource, trackClip, volumeLevel);
        }

        currentlyPlayingTrack = trackName;
        currentMinimumPlayDuration = minimumPlayDuration;
        timeSpentPlayingCurrentTrack = 0f;

    }

    private void PlayMusicOnSource(AudioSource audioSource, AudioClip trackClip, float targetVolume = 1f)
    {
        audioSource.clip = trackClip;
        audioSource.volume = 0f;
        audioSource.Play();
        audioSource.DOFade(targetVolume, crossfadeDuration);
    }

    internal static void PlaySound(GameObject target, string soundName)
    {
        SoundManager.instance.PlaySoundInternal(target, soundName);
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
            return;
        }

        // Play the sound as a one-shot
        audioSource.PlayOneShot(soundClip);
    }
}
