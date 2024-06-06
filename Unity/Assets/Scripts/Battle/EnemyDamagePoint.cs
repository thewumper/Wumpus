using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WumpusCore.Player;

public class EnemyDamagePoint : MonoBehaviour
{
    [SerializeField] private ValueBar playerHealth;
    [SerializeField] private ValueBar enemyHealth;
    [SerializeField] private int damage;
    [Range(0f, 1f)] [SerializeField] private float lifesteal;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            enemyHealth.Value -= damage;
            
            if (lifesteal > 0f && playerHealth.Value < playerHealth.MaxValue) 
            {
                playerHealth.Value += damage * lifesteal;
            }
        }
    }
}
