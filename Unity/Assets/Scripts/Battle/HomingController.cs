using UnityEngine;

namespace WumpusUnity.Battle
{
    public class HomingController : MovementController
    {
        [SerializeField] public Rigidbody2D target;
        [SerializeField] public float speed;
        [SerializeField] public float throttleDownRange;
        [SerializeField] public float timeout;
        private double startTime;
        private double endTime;

        void Init()
        {
            base.Init();
            startTime = Time.timeAsDouble;
            endTime = startTime + timeout;
        }
        
        void FixedUpdate()
        {
            if (timeout != 0 && endTime >= Time.timeAsDouble)
            {
                Destroy(this.gameObject);
                return;
            }
            
            Vector2 targetVector = target.position - rigidbody.position;
            Vector2 velocityOvershot = rigidbody.velocity - target.velocity;

            Vector2 velocityAdjust = velocityOvershot * Vector2.Dot(targetVector.normalized, velocityOvershot.normalized);

            Vector2 sumObjective = targetVector + velocityAdjust;
            
            if (sumObjective.magnitude > throttleDownRange)
            {
                // Max throttle
                acceleration = sumObjective.normalized * speed;
            }
            else
            {
                // Throttle down based on proximity
                acceleration = sumObjective * speed / throttleDownRange;
            }
            
            base.FixedUpdate();
        }
    }
}