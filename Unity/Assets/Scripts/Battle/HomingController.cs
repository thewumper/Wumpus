using UnityEngine;

namespace WumpusUnity.Battle
{
    public class HomingController : MovementController
    {
        [SerializeField] private Rigidbody2D target;
        [SerializeField] private float speed;
        [SerializeField] private float throttleDownRange;
        
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