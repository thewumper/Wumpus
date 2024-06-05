using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WumpusUnity.Battle;

public class BurstSpawner : SpawnerMode
{
    /// <summary>
    /// Objects' velocities
    /// </summary>
    [SerializeField] private float outputSpeed;
    /// <summary>
    /// Time (seconds) between each bullet output
    /// </summary>
    [SerializeField] private float outputDelay;
    /// <summary>
    /// Number of homing bullets fired
    /// </summary>
    [SerializeField] private int bulletCount;
    /// <summary>
    /// The amount (in radians) the output rotates each second
    /// </summary>
    [SerializeField] private double phaseShift;

    private double phase = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Room == null || Rigidbody == null)
        {
            // Oops! Not ready yet
            Debug.Log("Bypassed BurstSpawner Update method, mode is unprepared.");
            return;
        }
        
        phase += phaseShift * Time.fixedDeltaTime;
        if (phase > 2 * Math.PI)
        {
            phase -= 2 * Math.PI;
        }
        
        timeSinceLastOutput += Time.fixedDeltaTime;
        if (timeSinceLastOutput >= outputDelay)
        {
            timeSinceLastOutput = 0f;
            
            double angleInterval = Math.PI * 2 / bulletCount;
            
            if (HazardTypes == null || !HazardTypes.ContainsKey("Bullet"))
            {
                throw new KeyNotFoundException(
                    "This BurstSpawner mode cannot access the necessary bullet type. It may need a SpawnerController to function.");
            }

            for (int i = 0; i < bulletCount; i++)
            {
                double angle = phase + (i * angleInterval);

                GameObject prefab = this.HazardTypes["Bullet"];
            
                GameObject obj = Instantiate(prefab, Rigidbody.position, Quaternion.identity);
                obj.transform.SetParent(Room.transform);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                
                MovementController controller = obj.GetComponent<MovementController>();
                controller.velocityFalloff = 1f;
                controller.startingPosition = Rigidbody.position;
                controller.acceleration = Vector2.zero;
                    
                float x = (float)Math.Sin(angle);
                float y = (float)Math.Cos(angle);

                controller.startingVelocity = new Vector2(x, y).normalized * outputSpeed;
                controller.Init();
            }
        }
    }
}