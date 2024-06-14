using System;
using UnityEngine;

/// <summary>
/// Controls the smooth movement of an object
/// Controlled by a public acceleration, which smooths into the internal acceleration, which controls the rigidbody
/// </summary>
public class MovementController : MonoBehaviour
{
    [SerializeField] public new Rigidbody2D rigidbody;
    /// <summary>
    /// Internal acceleration is multiplied by this value each second before public acceleration control is added
    /// Lower values mean smoother (and wider) movement curves
    /// </summary>
    [Range(0f, 1f)] [SerializeField] public float accelerationFalloff = .75f;
    /// <summary>
    /// Internal velocity is multiplied by this value each second before acceleration is added
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] public float velocityFalloff = .85f;
    /// <summary>
    /// The position this will start in. Set before calling Init
    /// </summary>
    [SerializeField] public Vector2 startingPosition;
    /// <summary>
    /// The velocity this will start with. Set before calling Init
    /// </summary>
    [SerializeField] public Vector2 startingVelocity;
    // The actual current acceleration (smoothed from acceleration)
    private Vector2 currentAcceleration;
    /// <summary>
    /// The acceleration that you desire the movementController to undergo
    /// </summary>
    [HideInInspector] public Vector2 acceleration;
    
    /// <summary>
    /// Initializes the momvement controller based on presets
    /// </summary>
    public void Init()
    {
        rigidbody.position = startingPosition;
        rigidbody.velocity = startingVelocity;
        currentAcceleration = acceleration;
        rigidbody.inertia = float.MaxValue;
    }

    /// <summary>
    /// Updates the internal acceleration and velocity. Position is automatically updated by the rigidbody.
    /// </summary>
    protected void FixedUpdate()
    {
        currentAcceleration *= (float)Math.Pow(accelerationFalloff, 60f * Time.fixedDeltaTime);
        currentAcceleration += acceleration * (Time.fixedDeltaTime * 60f * (1 - accelerationFalloff));
        
        rigidbody.velocity *= (float)Math.Pow(velocityFalloff, Time.fixedDeltaTime);
        rigidbody.velocity += currentAcceleration * (Time.fixedDeltaTime * 60f * (1 - velocityFalloff));
    }
}
