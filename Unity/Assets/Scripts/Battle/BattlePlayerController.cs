using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("rb")] [SerializeField] private Rigidbody2D rigidbody;
    [Range(0f, 1f)] [SerializeField] private float m_AccelerationFalloff = .8f;
    [Range(0f, 1f)] [SerializeField] private float m_VelocityFalloff = .9f;
    [Range(-100f, 100f)] [SerializeField] private float m_AccelerationMax = 10f;
    [SerializeField] private LayerMask m_RoomCollider;
    [Range(0f, 10f)] [SerializeField] private float speed = 1.0f;
    private Vector2 m_Acceleration;

    private void Awake()
    {
        rigidbody.velocity = Vector2.zero;
        m_Acceleration = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        input *= speed * Time.deltaTime;
        // Acceleration slows over time
        m_Acceleration += input;
        m_Acceleration *= m_AccelerationFalloff;
        
        // Velocity follows acceleration, with simple drag
        rigidbody.velocity += m_Acceleration;
        rigidbody.velocity *= m_VelocityFalloff;
        //transform.Translate(rigidbody.velocity);
    }
    
    public void Move(float moveX, float moveY)
    {
        // Simply add more acceleration
        m_Acceleration += new Vector2(moveX, moveY);
        
        // Hard cap
        if (m_Acceleration.x > m_AccelerationMax)
        {
            m_Acceleration.x = m_AccelerationMax;
        }
        if (m_Acceleration.y > m_AccelerationMax)
        {
            m_Acceleration.y = m_AccelerationMax;
        }
        if (m_Acceleration.x < -m_AccelerationMax)
        {
            m_Acceleration.x = -m_AccelerationMax;
        }
        if (m_Acceleration.y < -m_AccelerationMax)
        {
            m_Acceleration.y = -m_AccelerationMax;
        }
    }
}
