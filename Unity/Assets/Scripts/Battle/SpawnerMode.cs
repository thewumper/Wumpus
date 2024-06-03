using System;
using System.Collections.Generic;
using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerMode : MonoBehaviour
    {
        protected GameObject Room;
        protected Rigidbody2D Rigidbody;
        protected Dictionary<String, GameObject> BulletTypes;

        public void Initialize(GameObject room, Rigidbody2D rigidbody, Dictionary<String, GameObject> bulletTypes)
        {
            this.Room = room;
            this.Rigidbody = rigidbody;
            this.BulletTypes = bulletTypes;
        }
    }
}