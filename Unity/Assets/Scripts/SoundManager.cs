using System;
using System.Linq;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

public class SoundManager : MonoBehaviour
{
 
    private GameObject northDoor;
    private GameObject northEastDoor;
    private GameObject southEastDoor;
    private GameObject southDoor;
    private GameObject southWestDoor;
    private GameObject northWestDoor;

    // The Sound Clips for each sound
    [SerializeField] private AudioClip wumpusClip;
    [SerializeField] private AudioClip luckyCatClip;
    [SerializeField] private AudioClip batClip;
    [SerializeField] private AudioClip ratClip;
    [SerializeField] private AudioClip acrobatClip;
    [SerializeField] private AudioClip vatClip;

    private Controller controller;

    public void Init(GameObject northDoor, GameObject northEastDoor,
        GameObject southEastDoor, GameObject southDoor, GameObject southWestDoor, GameObject northWestDoor)
    {
        controller = Controller.GlobalController;

        this.northDoor = northDoor;
        this.northEastDoor = northEastDoor;
        this.southEastDoor = southEastDoor;
        this.southDoor = southDoor;
        this.southWestDoor = southWestDoor;
        this.northWestDoor = northWestDoor;
    }

    /// <summary>
    /// Updates the current sounds that are playing in the room
    /// </summary>
    public void UpdateSoundState()
    {
        StopSound(northDoor);
        StopSound(northWestDoor);
        StopSound(southWestDoor);
        StopSound(southDoor);
        StopSound(southEastDoor);
        StopSound(northEastDoor);

        for (int i = 0; i < controller.GetHazardHints().Count; i++)
        {
            Controller.DirectionalHint directionalHint = controller.GetHazardHints()[i];

            PlaySound(directionalHint.Hazards.ToArray(), GetDoorFromDirection(directionalHint.Direction));
        }
    }

    /// <summary>
    /// Plays a single <see cref="RoomAnomaly"/> type at a given door.
    /// </summary>
    /// <param name="type">The <see cref="RoomAnomaly"/> type to play at the door.</param>
    /// <param name="door">The door to play the sound at.</param>
    /// <param name="overrideSound">If we should override the previous sound or keep adding new sounds</param>
    public void PlaySound(RoomAnomaly type, GameObject door, bool overrideSound = true)
    {
        if (door == null)
        {
            throw new InvalidOperationException("Door is null.");
        }

        // Get the component or create one if it doesn't exist.
        AudioSource doorAudioSource = door.GetComponent<AudioSource>();
        if (door.GetComponent<AudioSource>() == null || !overrideSound)
        {
            doorAudioSource = door.AddComponent<AudioSource>();
        }
        
        // Set the sound clip.
        doorAudioSource.clip = GetAudioClipFromType(type);

        // Set a few settings.
        doorAudioSource.loop = true;
        doorAudioSource.spatialBlend = 0.7f;
        doorAudioSource.volume = 1f;

        if (type == RoomAnomaly.Acrobat)
        {
            doorAudioSource.pitch = 0.8f;
        }

        // Play!
        doorAudioSource.Play();
    }
    
    /// <summary>
    /// Plays an array of <see cref="RoomAnomaly"/> types at a given door.
    /// This doesn't work yet, at the moment it will only play the last sound in the array.
    /// </summary>
    /// <param name="types">The array of <see cref="RoomAnomaly"/> types to play on the door.</param>
    /// <param name="door">The door to play the sounds at.</param>
    /// <exception cref="InvalidOperationException">When the door is null.</exception>
    public void PlaySound(RoomAnomaly[] types, GameObject door)
    {
        if (door == null)
        {
            throw new InvalidOperationException("Door is null.");
        }
        
        for (int i = 0; i < types.Length; i++)
        {
            PlaySound(types[i], door, false);
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
        
        AudioSource[] doorAudioSource = door.GetComponents<AudioSource>().ToArray();
        if (doorAudioSource == null)
        {
            throw new InvalidOperationException("Audio Source does not exist on door.");
        }

        // Stops the sound by making the audio clip null and stopping the sound
        for (int i = 0; i < doorAudioSource.Length; i++)
        {
            doorAudioSource[i].Stop();
            doorAudioSource[i].clip = null;
        }
    }

    /// <summary>
    /// Gets an audio clip from the given sound type.
    /// </summary>
    /// <param name="type">The <see cref="SoundType"/> type to find the associated <see cref="AudioClip"/> of.</param>
    /// <returns>The <see cref="AudioClip"/> clip associated with the given <see cref="SoundType"/>.</returns>
    /// <exception cref="InvalidOperationException">When the <see cref="SoundType"/> type does not have an <see cref="AudioClip"/> associated with it.</exception>
    private AudioClip GetAudioClipFromType(RoomAnomaly type)
    {
        switch (type)
        {
            case RoomAnomaly.Wumpus:
                return wumpusClip;
            case RoomAnomaly.Cat:
                return luckyCatClip;
            case RoomAnomaly.Bats:
                return batClip;
            case RoomAnomaly.Rat:
                return ratClip;
            case RoomAnomaly.Vat:
                return vatClip;
            case RoomAnomaly.Acrobat:
                return acrobatClip;
            default:
                throw new InvalidOperationException("Given RoomAnomaly does not have a sound clip associated with it.");
        }
    }

    private GameObject GetDoorFromDirection(Directions direction)
    {
        switch (direction)
        {
            case Directions.North:
                return northDoor;
            case Directions.NorthEast:
                return northEastDoor;
            case Directions.SouthEast:
                return southEastDoor;
            case Directions.South:
                return southDoor;
            case Directions.SouthWest:
                return southWestDoor;
            case Directions.NorthWest:
                return northWestDoor;
            default:
                throw new InvalidOperationException("Given Room Direction is not valid");
        }
    }
}

