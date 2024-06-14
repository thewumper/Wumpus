using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace WumpusUnity.Battle
{
    /// <summary>
    /// Represents a mode of functioning for the spawner. Intended to be extended
    /// </summary>
    public class SpawnerMode : MonoBehaviour
    {
        /// <summary>
        /// The room that the spawner is in
        /// </summary>
        protected GameObject Room;
        /// <summary>
        /// The spawner's rigidbody
        /// </summary>
        protected Rigidbody2D Rigidbody;
        /// <summary>
        /// Contains all bullet types and any other hazards
        /// </summary>
        protected Dictionary<String, GameObject> HazardTypes;
        /// <summary>
        /// The duration (sec) that this will run before the next mode is called in
        /// </summary>
        [SerializeField] public float duration;
        /// <summary>
        /// Whether or not the bullets launched should add the spawner's velocity to their own
        /// </summary>
        [SerializeField] public bool addSpawnerVelocity;
        /// <summary>
        /// The amount of time (sec) after the previous mode ends that will be waited before this mode begins
        /// </summary>
        [SerializeField] public float startingBuffer;

        protected float timeSinceLastOutput;

        protected Action update;

        /// <summary>
        /// Should be run each time the mode needs to be activated
        /// Do not set enabled to true
        /// </summary>
        public void Activate()
        {
            timeSinceLastOutput = float.MaxValue;
            if (duration == 0)
            {
                timeSinceLastOutput = float.MaxValue;
                update();
                timeSinceLastOutput = float.MinValue;
                enabled = false;
                return;
            }

            enabled = true;
        }

        /// <summary>
        /// Prepares the spawner mode with all necessary details
        /// </summary>
        /// <param name="room">The room in which the assigned spawner resides</param>
        /// <param name="rigidbody_">The rigidbody of the assigned spawner</param>
        /// <param name="hazardTypes">Contains all bullet types and any other hazards</param>
        public void Initialize(GameObject room, Rigidbody2D rigidbody_, Dictionary<String, GameObject> hazardTypes)
        {
            this.Room = room;
            this.Rigidbody = rigidbody_;
            this.HazardTypes = hazardTypes;
            this.enabled = false;
        }
    }
}