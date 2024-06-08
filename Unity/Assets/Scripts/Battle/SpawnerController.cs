using System;
using System.Collections.Generic;
using UnityEngine;

namespace WumpusUnity.Battle
{
    public class SpawnerController : MonoBehaviour
    {
        [SerializeField] private GameObject room;
        [SerializeField] private new Rigidbody2D rigidbody;
        
        // Hazard types
        [SerializeField] private GameObject prefabBullet;
        [SerializeField] private GameObject prefabHomingBullet;

        protected Dictionary<String, GameObject> HazardTypes;
        
        private SpawnerMode[] Modes;
        private int currentMode = -1;

        private float remainingTime;
        private bool waitingForBuffer = false;

        void Awake()
        {
            HazardTypes = new Dictionary<string, GameObject>()
            {
                {"Bullet", prefabBullet},
                {"HomingBullet", prefabHomingBullet}
            };
            
            SpawnerMode[] availableModes = GetComponents<SpawnerMode>();
            Debug.Log(availableModes.Length + " Modes found");
            Stack<SpawnerMode> addedModes = new Stack<SpawnerMode>();
            foreach (SpawnerMode mode in availableModes)
            {
                if (mode.enabled)
                {
                    mode.Initialize(room, rigidbody, HazardTypes);
                    addedModes.Push(mode);
                    Debug.Log("Initialized mode " + mode.GetType());
                }
            }
            Debug.Log(addedModes.Count + " Modes initialized");

            Modes = addedModes.ToArray();

            // Mode decided in Update
            remainingTime = 0f;
        }
        
        void FixedUpdate()
        {
            remainingTime -= Time.fixedDeltaTime;
            
            // Break if still waiting
            if (remainingTime > 0)
            {
                return;
            }
            
            if (waitingForBuffer)
            {
                waitingForBuffer = false;
                Modes[currentMode].Activate();
                remainingTime = Modes[currentMode].duration;
                return;
            }
            
            if (Modes.Length <= 1)
            {
                Modes[0].Activate();
                remainingTime = float.MaxValue;
                return;
            }
            
            int nextMode = currentMode;
            while (nextMode == currentMode)
            {
                nextMode = UnityEngine.Random.Range(0, Modes.Length);
            }

            foreach (SpawnerMode mode in Modes)
            {
                mode.enabled = false;
            }
            
            currentMode = nextMode;
            Debug.Log("Switching to mode " + currentMode + ", " + Modes[currentMode].GetType());

            if (Modes[currentMode].startingBuffer > 0.0)
            {
                waitingForBuffer = true;
                remainingTime = Modes[currentMode].startingBuffer;
                return;
            }
            
            Modes[currentMode].Activate();
            remainingTime = Modes[currentMode].duration;
        }
    }
}