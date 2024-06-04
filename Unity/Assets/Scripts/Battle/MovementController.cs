using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] public float velocityFalloff = .85f;

    [SerializeField] public Vector2 startingPosition;
    [SerializeField] public Vector2 startingVelocity;
    [SerializeField] public Vector2 acceleration;
    
    void Start()
    {
        rigidbody.position = startingPosition;
        rigidbody.velocity = startingVelocity;
        rigidbody.inertia = float.MaxValue;
    }

    void Update()
    {
        rigidbody.velocity += acceleration * Time.deltaTime;
        rigidbody.velocity *= (float)Math.Pow(velocityFalloff, Time.deltaTime);
    }
}
