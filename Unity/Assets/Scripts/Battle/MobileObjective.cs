using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WumpusUnity.Battle;

public class MobileObjective : MovementController
{
    [SerializeField] private Vector2 center;
    [SerializeField] private float apothem;
    [SerializeField] private float retargetDistance;
    [SerializeField] private float speed;

    private Vector2 target;
    
    private void Awake()
    {
        target = center;
        Relocate();
    }

    private new void FixedUpdate()
    {
        Vector2 targetApproach = target - rigidbody.position;
        
        if (targetApproach.magnitude < retargetDistance)
        {
            RelocateTarget();
        }

        acceleration = targetApproach.normalized * speed;
        
        base.FixedUpdate();
    }

    private void Relocate()
    {
        rigidbody.position = center + UnityEngine.Random.insideUnitCircle * apothem;
        RelocateTarget();
    }

    private void RelocateTarget()
    {
        target = center + UnityEngine.Random.insideUnitCircle * apothem;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Relocate();
        }
    }
}
