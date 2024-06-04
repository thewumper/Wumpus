using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WumpusUnity.Battle;

public class HomingCluster : SpawnerMode
{
    /// <summary>
    /// The object that the bullets track
    /// </summary>
    [SerializeField] private Rigidbody2D target;
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

            Vector2 direction = target.GetComponent<Rigidbody2D>().position - this.Rigidbody.position;

            // If just one bullet, fire straight down the middle
            if (bulletCount == 1)
            {
                GameObject obj = Instantiate(HazardTypes["HomingBullet"], Rigidbody.position, Quaternion.identity);
                obj.transform.SetParent(Room.transform);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                MovementController controller = obj.GetComponent<MovementController>();
                controller.velocityFalloff = 1f;
                controller.startingPosition = Rigidbody.position;
                controller.startingVelocity = Rigidbody.velocity + direction.normalized * outputSpeed;
                controller.acceleration = Vector2.zero;
                obj.GetComponent<HomingController>().target = target;
            }
            
            // If many bullets, fire in an even spread
            double middleAngle = Math.Atan2(direction.y, direction.x) - (float)Math.PI / 2f;
            double angleInterval = bulletSpread / (bulletCount - 1);
            double minAngle = middleAngle - (bulletSpread / 2);

            for (int i = 0; i < bulletCount; i++)
            {
                double angle = minAngle + (i * angleInterval);
                
                GameObject obj = Instantiate(HazardTypes["HomingBullet"], Rigidbody.position, Quaternion.identity);
                obj.transform.SetParent(Room.transform);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);

                float x = (float)Math.Sin(angle);
                float y = (float)Math.Cos(angle);

                MovementController controller = obj.GetComponent<MovementController>();
                controller.velocityFalloff = 1f;
                controller.startingPosition = Rigidbody.position;
                controller.startingVelocity = Rigidbody.velocity + new Vector2(x, y).normalized * outputSpeed;
                controller.acceleration = Vector2.zero;
                obj.GetComponent<HomingController>().target = target;
            }
        }
    }
}