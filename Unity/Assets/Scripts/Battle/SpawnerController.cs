using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

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
        private int currentMode = 0;

        private float remainingTime;

        void Start()
        {
            HazardTypes = new Dictionary<string, GameObject>()
            {
                {"Bullet", prefabBullet},
                {"HomingBullet", prefabHomingBullet}
            };
            
            Modes = GetComponents<SpawnerMode>();
            foreach (SpawnerMode mode in Modes)
            {
                mode.Initialize(room, rigidbody, HazardTypes);
            }

            // Mode decided in Update
            remainingTime = 0f;
        }
        
        void FixedUpdate()
        {
            remainingTime -= Time.fixedDeltaTime;
            if (remainingTime <= 0)
            {
                Modes[currentMode].enabled = false;
                int nextMode = currentMode;
                if (Modes.Length > 1)
                {
                    while (nextMode == currentMode)
                    {
                        nextMode = UnityEngine.Random.Range(0, Modes.Length);
                    }
                }
                else
                {
                    nextMode = 0;
                }

                currentMode = nextMode;
                Debug.Log("Switching to mode " + currentMode + ", " + Modes[currentMode].GetType());
                Modes[currentMode].enabled = true;
                remainingTime = Modes[currentMode].duration;
            }
        }
    }
}