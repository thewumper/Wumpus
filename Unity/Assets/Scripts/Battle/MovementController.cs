using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float velocityFalloff = .85f;

    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private Vector2 startingVelocity;
    [SerializeField] private Vector2 acceleration;
    
    void Start()
    {
        Debug.Log(rigidbody.position);
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
