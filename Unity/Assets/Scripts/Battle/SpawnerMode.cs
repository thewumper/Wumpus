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

        protected float timeSinceLastOutput;

        private void OnEnable()
        {
            timeSinceLastOutput = 0f;
        }
        
        public void Initialize(GameObject room, Rigidbody2D rigidbody_, Dictionary<String, GameObject> hazardTypes)
        {
            if (duration == 0)
            {
                Destroy(this.gameObject);
                throw new ConstraintException("Mode duration cannot equal zero.");
            }
            this.Room = room;
            this.Rigidbody = rigidbody_;
            this.HazardTypes = hazardTypes;
            this.enabled = false;
        }
    }
}