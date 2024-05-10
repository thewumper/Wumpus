namespace WumpusCore.Battle.BulletHell
{
    public interface ISpawner: IDisplayable
    {
        /// <summary>
        /// The spawner's position in space
        /// </summary>
        Vector2 Position { get; }
        
        /// <summary>
        /// The direction the spawner is moving in space
        /// </summary>
        Vector2 Velocity { get; }

        /// <summary>
        /// The direction the spawner is curving in its motion
        /// </summary>
        Vector2 Acceleration { get; }

        /// <summary>
        /// Advances the spawner in time
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        void Tick(double seconds);
    }
}