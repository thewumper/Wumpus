using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates an object at a configurable speed around an origin
/// Controls objects' velocities and positions to match
/// </summary>
public class Rotate : MonoBehaviour
{
    /// <summary>
    /// The rigidbody that will be rotated
    /// </summary>
    [SerializeField] private Rigidbody2D rigidbody;
    /// <summary>
    /// The center point that the object should rotate around
    /// </summary>
    [SerializeField] private Vector2 center;
    /// <summary>
    /// The radius that the object should keep from the center
    /// </summary>
    [SerializeField] private float radius;
    /// <summary>
    /// The speed at which the object should move around the circumference
    /// </summary>
    [SerializeField] private float speed;
    private float angle;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.position = new Vector2(center.x - radius, center.y);
        rigidbody.velocity = new Vector2(0, speed);
        angle = - (float)Math.PI / 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle += (speed * Time.fixedDeltaTime) / radius;
        if (angle > 2 * Math.PI)
        {
            angle -= 2 * (float)Math.PI;
        }
        
        Vector2 pos = new Vector2((float)Math.Sin(angle) * radius, (float)Math.Cos(angle) * radius);
        float velAngle = angle + (float)Math.PI / 2f;
        Vector2 vel = new Vector2((float)Math.Sin(velAngle), (float)Math.Cos(velAngle)) * speed;
        rigidbody.position = pos + center;
        rigidbody.rotation = 0;
        rigidbody.velocity = vel;
    }
}
