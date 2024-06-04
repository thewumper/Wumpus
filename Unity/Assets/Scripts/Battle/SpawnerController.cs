using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WumpusUnity.Battle
{
    public class SpawnerController : MonoBehaviour
    {
        [SerializeField] private GameObject Room;
        [SerializeField] private Rigidbody2D Rigidbody;
        [SerializeField] private GameObject Prefab;
        
        private SpawnerMode[] Modes;
        private int currentMode = 0;

        private float remainingTime;

        void Start()
        {
            Modes = GetComponents<SpawnerMode>();
            foreach (SpawnerMode mode in Modes)
            {
                mode.Initialize(Room, this.Rigidbody, Prefab);
            }

            // Mode decided in Update
            remainingTime = 0f;
        }
        
        void Update()
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                Modes[currentMode].enabled = false;
                int nextMode = currentMode;
                while (nextMode == currentMode)
                {
                    nextMode = UnityEngine.Random.Range(0, Modes.Length);
                }

                currentMode = nextMode;
                Debug.Log("Switching to mode " + currentMode + ", " + Modes[currentMode].name);
                Modes[currentMode].enabled = true;
                remainingTime = Modes[currentMode].duration;
            }
        }
    }
}