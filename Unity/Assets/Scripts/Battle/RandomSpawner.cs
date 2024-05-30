using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private GameObject prefab;
    /// <summary>
    /// Objects' velocities
    /// </summary>
    [SerializeField] private float outputSpeed;
    /// <summary>
    /// Time (seconds) between each bullet output
    /// </summary>
    [SerializeField] private float outputDelay;

    private float timeSinceLastOutput;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastOutput += Time.deltaTime;
        if (timeSinceLastOutput >= outputDelay)
        {
            timeSinceLastOutput = 0f;
            
            GameObject obj = Instantiate(prefab, rigidbody.position, Quaternion.identity);
            obj.transform.SetParent(room.transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);

            float x = (float)UnityEngine.Random.value - 0.5f;
            float y = (float)UnityEngine.Random.value - 0.5f;
            MovementController controller = obj.GetComponent<MovementController>();
            controller.velocityFalloff = 1f;
            controller.startingPosition = rigidbody.position;
            controller.startingVelocity = rigidbody.velocity + new Vector2(x, y).normalized * outputSpeed;
            controller.acceleration = Vector2.zero;
        }
    }
}
