using System;
using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerController
    {
        [SerializeField] private GameObject Room;
        [SerializeField] private new Rigidbody2D Rigidbody;
        [SerializeField] private GameObject Prefab;
        
        /// <summary>
        /// Pairs of spawner mode components and the time they remain when active
        /// </summary>
        [SerializeField] private (SpawnerMode, float)[] Modes;

        private float remainingTime;

        void Start()
        {
            foreach ((SpawnerMode, float) mode in Modes)
            {
                mode.Item1.Initialize(Room, Rigidbody, Prefab);
            }
        }
        
        void Update()
        {
            //
        }
        
    }
}