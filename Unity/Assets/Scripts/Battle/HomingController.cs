using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WumpusUnity.Battle
{
    public class HomingController : MovementController
    {
        [SerializeField] public Rigidbody2D target;
        [SerializeField] public float speed;
        /// <summary>
        /// Radius within which the velocity match algorithm applies, and outside of which the bullet exclusively accelerates directly forward
        /// </summary>
        [SerializeField] public float velocityMatchRadius = 50f;
        [SerializeField] public float timeout;
        private double startTime;
        private double endTime;

        // The assumed maximum speed of the target
        private float targetMaxSpeed = 1f;
        
        private static readonly double TAU = Math.PI * 2;

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

            float targetSpeed = target.velocity.magnitude;
            if (targetSpeed > targetMaxSpeed)
            {
                targetMaxSpeed = targetSpeed;
            }
            
            Vector2 targetRelative = target.position - rigidbody.position;
            Vector2 seekVector = targetRelative.normalized;

            if (targetRelative.magnitude > velocityMatchRadius)
            {
                // Outside of velocity match radius
                acceleration = seekVector;
            }
            else
            {
                // Velocity match algorithm
                // Restricts chosen vector to within 45 degrees of that which points directly at target
                // Otherwise gets as close to the target's velocity as possible
                Vector2 matchVector = (target.velocity - rigidbody.velocity).normalized;
                double seekAngle = Math.Atan2(seekVector.x, seekVector.y);
                double matchAngle = Math.Atan2(matchVector.x, matchVector.y);

                double angleRange = Math.PI / 2.0;

                double relativeMatchAngle = AngleMod(matchAngle - seekAngle);
                double accelerationAngle;
                
                if (relativeMatchAngle < matchAngle || relativeMatchAngle > TAU - matchAngle)
                {
                    // Within acceptable range, return as is
                    accelerationAngle = matchAngle;
                } 
                else if (relativeMatchAngle < Math.PI)
                {
                    accelerationAngle = AngleMod(TAU + angleRange + seekAngle);
                }
                else
                {
                    accelerationAngle = AngleMod(TAU - angleRange + seekAngle);
                }
                
                float x = (float)Math.Sin(accelerationAngle);
                float y = (float)Math.Cos(accelerationAngle);

                Vector2 restrainedMatchVector = new Vector2(x, y);

                // Lerp into velocity matching as target speed increases
                float matchFactor = targetSpeed / targetMaxSpeed;
                if (matchFactor < 0.2f)
                {
                    matchFactor = 0.2f;
                }
                acceleration = matchFactor * restrainedMatchVector + (1 - matchFactor) * seekVector;
            }

            acceleration = acceleration.normalized * speed;
            base.FixedUpdate();
        }

        private static double AngleMod(double angle)
        {
            while (angle < 0)
            {
                angle += TAU;
            }
            
            while (angle >= TAU)
            {
                angle -= TAU;
            }

            return angle;
        }
    }
}