using System;
using System.Collections.Generic;
using UnityEngine;
using WumpusCore.Topology;

public class SoundManager
{
    private AudioClip wumpusClip;

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
        AudioSource doorAudioSource = door.GetComponent<AudioSource>();
        if (door.GetComponent<AudioSource>() == null)
        {
            doorAudioSource = door.AddComponent<AudioSource>();
        }

        if (type == SoundType.Wumpus)
        {
            doorAudioSource.clip = wumpusClip;
            doorAudioSource.loop = true;
            doorAudioSource.spatialBlend = 1.0f;
        }

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

    public void StopSound(GameObject door)
    {

    }

}

