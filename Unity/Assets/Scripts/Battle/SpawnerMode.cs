using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerMode : MonoBehaviour
    {
        protected GameObject Room;
        protected Rigidbody2D Rigidbody;
        protected Dictionary<String, GameObject> HazardTypes;
        [SerializeField] public float duration;
        [SerializeField] public bool addSpawnerVelocity;
        [SerializeField] public float startingBuffer;

        protected float timeSinceLastOutput;

        protected Action update;

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

        public void Initialize(GameObject room, Rigidbody2D rigidbody_, Dictionary<String, GameObject> hazardTypes)
        {
            this.Room = room;
            this.Rigidbody = rigidbody_;
            this.HazardTypes = hazardTypes;
            this.enabled = false;
        }
    }
}