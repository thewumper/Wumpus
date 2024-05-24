using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    /// <summary>
    /// Higher values smooth out acceleration more
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float accelerationFalloff = .95f;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float velocityFalloff = .98f;
    /// <summary>
    /// Higher values mean slower turning
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float rotationFalloff = .9f;
    /// <summary>
    /// The minimum velocity at which rotation continues to update
    /// </summary>
    [Range(0f, 0.1f)] [SerializeField] private float rotationUpdateCutoff = .01f;
    /// <summary>
    /// Absolute maximum allowable acceleration. Should never be reached
    /// </summary>
    [Range(-100f, 100f)] [SerializeField] private float accelerationMax = 100f;
    /// <summary>
    /// Factor of random rotation to allow player to flip around
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float shake = 0.001f;
    /// <summary>
    /// The collider of the room the player resides in
    /// </summary>
    [SerializeField] private LayerMask roomCollider;
    /// <summary>
    /// The rate at which the player can accelerate
    /// </summary>
    [Range(0f, 50f)] [SerializeField] private float speed = 8.0f;
    private Vector2 acceleration;
    private Vector2 orientation;

    private void Awake()
    {
        rigidbody.velocity = Vector2.zero;
        acceleration = Vector2.zero;
        orientation = Vector2.up;
        rigidbody.inertia = float.MaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        // Normalize input and scale by dT
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        input *= speed * Time.deltaTime;
        
        // Acceleration slows over time
        acceleration *= accelerationFalloff;
        
        // Take in input
        acceleration += input;
        
        // Hard cap on acceleration
        if (acceleration.x > accelerationMax)
        {
            acceleration.x = accelerationMax;
        }
        if (acceleration.y > accelerationMax)
        {
            acceleration.y = accelerationMax;
        }
        if (acceleration.x < -accelerationMax)
        {
            acceleration.x = -accelerationMax;
        }
        if (acceleration.y < -accelerationMax)
        {
            acceleration.y = -accelerationMax;
        }
        
        // Velocity follows acceleration, with simple drag
        rigidbody.velocity += acceleration;
        rigidbody.velocity *= velocityFalloff;
        
        // Update rotation
        Vector2 currentDirection = rigidbody.velocity.normalized;
        if (rigidbody.velocity.magnitude > rotationUpdateCutoff)
        {
            orientation += UnityEngine.Random.insideUnitCircle.normalized * shake;
            orientation.Normalize();
            orientation *= rotationFalloff;
            orientation += currentDirection * (1 - rotationFalloff);
            orientation.Normalize();
        }

        if (orientation.magnitude == 0)
        {
            orientation = Vector2.up;
        }
        
        // Set rotation
        rigidbody.rotation = (float)(180.0 * Math.Atan2(-orientation.x, orientation.y) / Math.PI);
    }
}
