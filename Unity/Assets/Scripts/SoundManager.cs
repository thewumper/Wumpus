using System;
using System.Collections.Generic;
using UnityEngine;
using WumpusCore.Topology;

public class SoundManager
{
    private AudioClip wumpusClip;
    private AudioClip luckyCatClip;
    private AudioClip batsClip;
    private AudioClip ratsClip;

    public SoundManager(AudioClip wumpusClip)
    {
        this.wumpusClip = wumpusClip;
    }

    public enum SoundType
    {
        Wumpus,
        LuckyCat,
        Bats,
        Rats
    }

    /// <summary>
    /// Plays a single sound type at the given door
    /// </summary>
    /// <param name="type">The sound type</param>
    /// <param name="door">The door to play the sound</param>
    public void PlaySound(SoundType type, GameObject door)
    {
        if (door == null)
        {
            throw new InvalidOperationException("Door is null.");
        }

        // Get the component or create one if it doesn't exist.
        AudioSource doorAudioSource = door.GetComponent<AudioSource>();
        if (door.GetComponent<AudioSource>() == null)
        {
            doorAudioSource = door.AddComponent<AudioSource>();
        }
        
        // Set the sound clip
        doorAudioSource.clip = GetAudioClipFromType(type);

        // Set a few settings
        doorAudioSource.loop = true;
        doorAudioSource.spatialBlend = 1.0f;

        // Play!
        doorAudioSource.Play();
    }

    /// <summary>
    /// Plays all the given sound types at the given door
    /// </summary>
    /// <param name="types">An array of sound types</param>
    /// <param name="door">The door to play it at</param>
    public void PlaySound(SoundType[] types, GameObject door)
    {
        for (int i = 0; i < types.Length; i++)
        {
            PlaySound(types[i], door);
        }
    }

    /// <summary>
    /// Stops playing the sound on the specified door
    /// </summary>
    /// <param name="door">The door to stop playing the sound on</param>
    public void StopSound(GameObject door)
    {
        if (door == null)
        {
            throw new InvalidOperationException("Door is null.");
        }
        
        AudioSource doorAudioSource = door.GetComponent<AudioSource>();
        if (doorAudioSource == null)
        {
            throw new InvalidOperationException("Audio Source does not exist on door.");
        }

        // Stops the sound by making the audio clip null and stopping the sound
        doorAudioSource.Stop();
        doorAudioSource.clip = null;
    }

    /// <summary>
    /// Gets an audio clip from the given sound type
    /// </summary>
    /// <param name="type">The given sound type</param>
    /// <returns>An audio clip</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private AudioClip GetAudioClipFromType(SoundType type)
    {
        switch (type)
        {
            case SoundType.Wumpus:
                return wumpusClip;
            default:
                throw new InvalidOperationException("Given sound type does not have a sound clip associated with it.");
        }
    }
}

