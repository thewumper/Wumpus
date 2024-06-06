using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WumpusUnity.Battle;
using Vector2 = UnityEngine.Vector2;

public class BattlePlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    /// <summary>
    /// Higher values smooth out acceleration more
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float accelerationFalloff = .75f;
    /// <summary>
    /// Higher values mean more slippery floors
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float velocityFalloff = .85f;
    /// <summary>
    /// Higher values mean slower turning
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float rotationFalloff = .6f;
    /// <summary>
    /// The minimum velocity at which rotation continues to update
    /// </summary>
    [Range(0f, 2f)] [SerializeField] private float rotationUpdateCutoff = 1.2f;
    /// <summary>
    /// Absolute maximum allowable acceleration. Should never be reached
    /// </summary>
    [Range(0f, 500f)] [SerializeField] private float accelerationMax = 100f;
    /// <summary>
    /// Factor of random rotation to allow player to flip around
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float shake = 0.001f;
    /// <summary>
    /// The rate at which the player can accelerate
    /// </summary>
    [Range(0f, 100f)] [SerializeField] private float speed = 20.0f;
    private Vector2 _acceleration;
    private Vector2 _orientation;

    [SerializeField] private ValueBar playerHealth;
    [SerializeField] private ValueBar enemyHealth;

    [SerializeField] private bool destroyOnContact;
    [Range(0f, 1f)] [SerializeField] private float minOpacity;
    [SerializeField] private float totalImmunityTime;
    private float _remainingImmunityTime;
    [Range(0f, 1f)] [SerializeField] private float lifesteal;

    [SerializeField] private new SpriteRenderer renderer;

    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private FadeIn cover;

    public bool GameEnded { get; private set; }
    public bool? Won { get; private set; }
    
    public bool ReadyToUnload { get; private set; }

    void Start()
    {
        rigidbody.velocity = Vector2.zero;
        _acceleration = Vector2.zero;
        _orientation = Vector2.up;
        rigidbody.inertia = float.MaxValue;
        GameEnded = false;
        Won = null;
        endGame = VisualGameOver;
        ReadyToUnload = false;
    }

    void FixedUpdate()
    {
        if (GameEnded)
        {
            if (cover.FadeComplete)
            {
                Destroy(spawner);
                Destroy(enemy);
                ReadyToUnload = true;
            }
            return;
        }
        
        // Check for game end
        if (playerHealth.Value <= 0f)
        {
            OnDeath();
        }

        if (enemyHealth.Value <= 0f)
        {
            OnVictory();
        }
        
        // Normalize input and scale by dT
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        input *= speed / (accelerationFalloff * velocityFalloff) * 60f * Time.fixedDeltaTime;
        
        // Acceleration slows over time
        _acceleration *= (float)Math.Pow(accelerationFalloff, 60f * Time.fixedDeltaTime);
        
        // Take in input
        _acceleration += input;
        
        // Hard cap on acceleration
        if (_acceleration.x > accelerationMax)
        {
            _acceleration.x = accelerationMax;
        }
        if (_acceleration.y > accelerationMax)
        {
            _acceleration.y = accelerationMax;
        }
        if (_acceleration.x < -accelerationMax)
        {
            _acceleration.x = -accelerationMax;
        }
        if (_acceleration.y < -accelerationMax)
        {
            _acceleration.y = -accelerationMax;
        }
        
        // Velocity follows acceleration, with simple drag
        rigidbody.velocity += _acceleration * (60f * Time.fixedDeltaTime);
        rigidbody.velocity *= (float)Math.Pow(velocityFalloff, 60f * Time.fixedDeltaTime);
        
        // Update rotation
        Vector2 currentDirection = rigidbody.velocity.normalized;
        if (rigidbody.velocity.magnitude > rotationUpdateCutoff)
        {
            _orientation += UnityEngine.Random.insideUnitCircle.normalized * shake;
            _orientation.Normalize();
            _orientation *= (float)Math.Pow(rotationFalloff, 60f * Time.fixedDeltaTime);
            _orientation += currentDirection * ((1 - rotationFalloff) * 60f * Time.fixedDeltaTime);
            _orientation.Normalize();
        }

        if (_orientation.magnitude == 0)
        {
            _orientation = Vector2.up;
        }
        
        // Set rotation
        rigidbody.rotation = (float)(180.0 * Math.Atan2(-_orientation.x, _orientation.y) / Math.PI);
        
        // Count down immunity
        _remainingImmunityTime -= Time.fixedDeltaTime;
        if (_remainingImmunityTime <= 0)
        {
            _remainingImmunityTime = 0;
            
            renderer.color = Color.white;
        }
        else
        {
            renderer.color = new Color(0.6f, 0.8f, 1.0f, (1 - (_remainingImmunityTime / totalImmunityTime)) * (1.0f - minOpacity) + minOpacity);
        }
    }
    
    // Colliding with bad things
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_remainingImmunityTime > 0)
        {
            return;
        }
        
        if (collision.gameObject.GetComponent<Damage>() != null)
        {
            playerHealth.Value -= collision.gameObject.GetComponent<Damage>().damage;
            
            if (lifesteal > 0f && enemyHealth.Value < enemyHealth.MaxValue) 
            {
                enemyHealth.Value += collision.gameObject.GetComponent<Damage>().damage * lifesteal;
            }
            
            _remainingImmunityTime = totalImmunityTime;
            if (destroyOnContact)
            {
                Destroy(collision.gameObject);
            }
        }
    }
    
    // Calls on player death
    public void OnDeath()
    {
        Won = false;
        GameOver();
    }

    // Calls on player win
    public void OnVictory()
    {
        Won = true;
        GameOver();
    }

    private static Action endGame;

    /// <summary>
    /// Calls once the player loses
    /// Unloads the scene
    /// </summary>
    public static void GameOver()
    {
        endGame();
    }

    private void VisualGameOver()
    {
        Debug.Log("Fading out");
        GameEnded = true;
        spawner.GetComponent<SpawnerController>().enabled = false;
        playerHealth.writable = false;
        enemyHealth.writable = false;
    }
}
