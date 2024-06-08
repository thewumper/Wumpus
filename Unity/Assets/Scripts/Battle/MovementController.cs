using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public new Rigidbody2D rigidbody;
    [Range(0f, 1f)] [SerializeField] public float accelerationFalloff = .75f;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] public float velocityFalloff = .85f;

    [SerializeField] public Vector2 startingPosition;
    [SerializeField] public Vector2 startingVelocity;
    // The actual current acceleration (smoothed from acceleration)
    private Vector2 currentAcceleration;
    /// <summary>
    /// The acceleration that you desire the movementController to undergo
    /// </summary>
    [HideInInspector] public Vector2 acceleration;
    
    public void Init()
    {
        rigidbody.position = startingPosition;
        rigidbody.velocity = startingVelocity;
        currentAcceleration = acceleration;
        rigidbody.inertia = float.MaxValue;
    }

    protected void FixedUpdate()
    {
        currentAcceleration *= (float)Math.Pow(accelerationFalloff, 60f * Time.fixedDeltaTime);
        currentAcceleration += acceleration * (Time.fixedDeltaTime * 60f * (1 - accelerationFalloff));
        
        rigidbody.velocity *= (float)Math.Pow(velocityFalloff, Time.fixedDeltaTime);
        rigidbody.velocity += currentAcceleration * (Time.fixedDeltaTime * 60f * (1 - velocityFalloff));
    }
}
