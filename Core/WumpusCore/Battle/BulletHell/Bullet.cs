namespace WumpusCore.Battle.BulletHell
{
    /// <summary>
    /// A hazardous object in 2d space
    /// </summary>
    public class Bullet
    {
        public readonly int DisplayID;
        
        /// <summary>
        /// The bullet's position in space
        /// </summary>
        public Vector2 Position { get; private set; }
        
        /// <summary>
        /// The direction the vector is moving in space
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// The direction the vector is curving in its motion
        /// </summary>
        public Vector2 Acceleration { get; private set; }

        /// <summary>
        /// The amount of health that impact with this bullet reduces from the player
        /// </summary>
        public int Damage;

        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, int damage, int displayID)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Damage = damage;
            DisplayID = displayID;
        }

        /// <summary>
        /// Advances the bullet in time
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        public void Tick(double seconds)
        {
            Position.Increment(Velocity);
            Velocity.Increment(Acceleration);
        }
    }
}