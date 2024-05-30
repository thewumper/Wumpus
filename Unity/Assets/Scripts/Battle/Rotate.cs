using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Vector2 center;
    [SerializeField] private float radius;
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
    void Update()
    {
        angle += (speed * Time.deltaTime) / radius;
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
