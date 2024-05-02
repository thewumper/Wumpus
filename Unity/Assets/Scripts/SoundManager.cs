using System;
using UnityEngine;

public class SoundManager
{
    private AudioClip wumpusClip;
    private AudioClip luckyCatClip;
    private AudioClip batsClip;
    private AudioClip ratsClip;
    
    /// <summary>
    /// Manages everything to do with audio playing.
    /// </summary>
    /// <param name="wumpusClip">The <see cref="AudioClip"/> clip to play for a wumpus.</param>
    public SoundManager(AudioClip wumpusClip)
    {
        this.wumpusClip = wumpusClip;
    }
    
    /// <summary>
    /// Contains all possible types of sounds.
    /// </summary>
    public enum SoundType
    {
        Wumpus,
        LuckyCat,
        Bats,
        Rats,
        Vats,
        Acrobats
    }

    /// <summary>
    /// Plays a single <see cref="SoundType"/> type at a given door.
    /// </summary>
    /// <param name="type">The <see cref="SoundType"/> type to play at the door.</param>
    /// <param name="door">The door to play the sound at.</param>
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
        
        // Set the sound clip.
        doorAudioSource.clip = GetAudioClipFromType(type);

        // Set a few settings.
        doorAudioSource.loop = true;
        doorAudioSource.spatialBlend = 1.0f;

        // Play!
        doorAudioSource.Play();
    }
    
    // TODO: Make work
    /// <summary>
    /// Plays an array of <see cref="SoundType"/> types at a given door.
    /// This doesn't work yet, at the moment it will only play the last sound in the array.
    /// </summary>
    /// <param name="types">The array of <see cref="SoundType"/> types to play on the door.</param>
    /// <param name="door">The door to play the sounds at.</param>
    /// <exception cref="InvalidOperationException">When the door is null.</exception>
    public void PlaySound(SoundType[] types, GameObject door)
    {
        if (door == null)
        {
            throw new InvalidOperationException("Door is null.");
        }
        
        for (int i = 0; i < types.Length; i++)
        {
            PlaySound(types[i], door);
        }
    }

    /// <summary>
    /// Stops playing all sounds on the specified door.
    /// </summary>
    /// <param name="door">The door to stop playing the sound on.</param>
    /// <exception cref="InvalidOperationException">When the door is null, or an <see cref="AudioSource"/> source does not exist on the door.</exception>
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
    /// Gets an audio clip from the given sound type.
    /// </summary>
    /// <param name="type">The <see cref="SoundType"/> type to find the associated <see cref="AudioClip"/> of.</param>
    /// <returns>The <see cref="AudioClip"/> clip associated with the given <see cref="SoundType"/>.</returns>
    /// <exception cref="InvalidOperationException">When the <see cref="SoundType"/> type does not have an <see cref="AudioClip"/> associated with it.</exception>
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

