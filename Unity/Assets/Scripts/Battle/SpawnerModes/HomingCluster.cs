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
    [SerializeField] private float speed;
    [SerializeField] private float velocityMatchRadius;
    [Range(0f, 1f)] [SerializeField] public float accelerationFalloff = .75f;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float velocityFalloff = .85f;
    /// <summary>
    /// The amount of time (in seconds) that the bullets will persist in the playing field
    /// </summary>
    [SerializeField] public float timeout;
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
    [Range(0f, 6.28319f)] [SerializeField] private float bulletSpread;

    void Awake()
    {
        update = FixedUpdate;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Room == null || Rigidbody == null)
        {
            // Oops! Not ready yet
            Debug.Log("Bypassed HomingCluster Update method, mode is unprepared.");
            return;
        }
        
        timeSinceLastOutput += Time.fixedDeltaTime;
        if (timeSinceLastOutput >= outputDelay)
        {
            timeSinceLastOutput = 0f;

            Vector2 direction = target.GetComponent<Rigidbody2D>().position - this.Rigidbody.position;

            // If just one bullet, fire straight down the middle
            if (bulletCount == 1)
            {
                HomingController controller = initBullet();
                controller.startingVelocity = direction.normalized * outputSpeed;
                if (addSpawnerVelocity)
                {
                    controller.startingVelocity += Rigidbody.velocity;
                }
                controller.Init();
            }
            else
            {
                // If many bullets, fire in an even spread
                double middleAngle = Math.Atan2(direction.x, direction.y);
                double angleInterval = bulletSpread / (bulletCount - 1);
                double minAngle = middleAngle - (bulletSpread / 2);

                for (int i = 0; i < bulletCount; i++)
                {
                    double angle = minAngle + (i * angleInterval);

                    HomingController controller = initBullet();
                    
                    float x = (float)Math.Sin(angle);
                    float y = (float)Math.Cos(angle);

                    controller.startingVelocity = new Vector2(x, y).normalized * outputSpeed;
                    if (addSpawnerVelocity)
                    {
                        controller.startingVelocity += Rigidbody.velocity;
                    }
                    controller.Init();
                }
            }
        }
    }

    /// <summary>
    /// Creates a new bullet based on the spawner's position and provided values
    /// </summary>
    /// <returns>A new HomingController representing a bullet</returns>
    private HomingController initBullet()
    {
        if (HazardTypes == null || !HazardTypes.ContainsKey("Bullet"))
        {
            throw new KeyNotFoundException(
                "This HomingCluster mode cannot access the necessary bullet type. It may need a SpawnerController to function.");
        }
        
        GameObject obj = Instantiate(HazardTypes["HomingBullet"], Rigidbody.position, Quaternion.identity);
        obj.transform.SetParent(Room.transform);
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        
        HomingController controller = obj.GetComponent<HomingController>();
        controller.accelerationFalloff = accelerationFalloff;
        controller.velocityFalloff = velocityFalloff;
        controller.startingPosition = Rigidbody.position;
        controller.acceleration = Vector2.zero;
        controller.timeout = timeout;
        controller.speed = speed;
        controller.velocityMatchRadius = velocityMatchRadius;
        controller.target = target;

        return controller;
    }
}