/*using System;
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
    [SerializeField] private 

    // Update is called once per frame
    void Update()
    {
        if (Room == null || Rigidbody == null)
        {
            // Oops! Not ready yet
            Debug.Log("Bypassed BurstSpawner Update method, mode is unprepared.");
            return;
        }
        
        timeSinceLastOutput += Time.deltaTime;
        if (timeSinceLastOutput >= outputDelay)
        {
            timeSinceLastOutput = 0f;
            
            if (HazardTypes == null || !HazardTypes.ContainsKey("Bullet"))
            {
                throw new KeyNotFoundException(
                    "This BurstSpawner mode cannot access the necessary bullet type. It may need a SpawnerController to function.");
            }
            GameObject prefab = this.HazardTypes["Bullet"];
            
            GameObject obj = Instantiate(prefab, Rigidbody.position, Quaternion.identity);
            obj.transform.SetParent(Room.transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);

            float x = (float)UnityEngine.Random.value - 0.5f;
            float y = (float)UnityEngine.Random.value - 0.5f;
            MovementController controller = obj.GetComponent<MovementController>();
            controller.velocityFalloff = 1f;
            controller.startingPosition = Rigidbody.position;
            controller.startingVelocity = Rigidbody.velocity + new Vector2(x, y).normalized * outputSpeed;
            controller.acceleration = Vector2.zero;
        }
    }
}
*/