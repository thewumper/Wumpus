using UnityEngine;

namespace WumpusUnity.Battle
{
    public class HomingController : MovementController
    {
        [SerializeField] public Rigidbody2D target;
        [SerializeField] public float speed;
        [SerializeField] public float throttleDownRange;
        
        void Update()
        {
            Vector2 targetVector = target.position - rigidbody.position;
            if (targetVector.magnitude > throttleDownRange)
            {
                // Max throttle
                acceleration = targetVector.normalized * speed;
            }
            else
            {
                // Throttle down based on proximity
                acceleration = targetVector * speed / throttleDownRange;
            }
            
            base.Update();
        }
    }
}