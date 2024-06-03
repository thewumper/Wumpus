using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace WumpusUnity.Battle
{
    public class SpawnerController : MonoBehaviour
    {
        [SerializeField] private GameObject Room;
        [SerializeField] private new Rigidbody2D Rigidbody;
        [FormerlySerializedAs("Prefab")] [SerializeField] protected GameObject Bullet;
        [SerializeField] protected GameObject HomingBullet;

        protected Dictionary<String, GameObject> BulletTypes;
        
        /// <summary>
        /// Points to each spawner mode component
        /// </summary>
        [SerializeField] private SpawnerMode[] Modes;

        private float remainingTime;

        void Start()
        {
            BulletTypes = new Dictionary<string, GameObject>()
            {
                { "Bullet", Bullet },
                { "HomingBullet", HomingBullet }
            };
            
            foreach (SpawnerMode mode in Modes)
            {
                mode.Initialize(Room, Rigidbody, BulletTypes);
            }
        }
        
        void Update()
        {
            //
        }
        
    }
}