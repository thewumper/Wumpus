using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WumpusUnity.Battle;

public class RandomSpawner : SpawnerMode
{
    /// <summary>
    /// Objects' velocities
    /// </summary>
    [SerializeField] private float outputSpeed;
    /// <summary>
    /// Time (seconds) between each bullet output
    /// </summary>
    [SerializeField] private float outputDelay;

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
            
            GameObject obj = Instantiate(Prefab, Rigidbody.position, Quaternion.identity);
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
