namespace WumpusCore.Battle.BulletHell
{
    /// <summary>
    /// A hazardous object in 2d space
    /// </summary>
    public class Bullet: IDisplayable
    {
        /// <summary>
        /// Represents the type of bullet
        /// </summary>
        public int DisplayID { get; }
        
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
        public int Damage { get; private set; }
        
        /// <summary>
        /// The size of the bullet
        /// </summary>
        public double Size { get; private set; }

        /// <summary>
        /// Constructs a Bullet
        /// </summary>
        /// <param name="position">The position of the Bullet</param>
        /// <param name="velocity">The speed and direction that the Bullet moves in</param>
        /// <param name="acceleration">The magnitude and direction of the Bullet's acceleration. Recommended to be 0.</param>
        /// <param name="damage">The amount of damage the Bullet inflicts upon contact with the player</param>
        /// <param name="displayID">A number representing the sprite of the bullet, as well as any effects it may have</param>
        /// <param name="size">The size of the bullet. Changes at what point the bullet connects with the player.</param>
        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, int damage, int displayID, double size)
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