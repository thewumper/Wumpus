using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WumpusUnity.Battle;

public class HomingCluster : SpawnerMode
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
    /// Total spread of bullets (angle from leftmost to rightmost bullet)
    /// </summary>
    [SerializeField] private float bulletSpread;

    private float timeSinceLastOutput;

    private void OnEnable()
    {
        timeSinceLastOutput = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastOutput += Time.deltaTime;
        if (timeSinceLastOutput >= outputDelay)
        {
            timeSinceLastOutput = 0f;
            
            GameObject obj = Instantiate(BulletTypes["HomingBullet"], Rigidbody.position, Quaternion.identity);
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