namespace WumpusCore.Battle.BulletHell
{
    public interface ISpawner: IDisplayable
    {
        /// <summary>
        /// The position of the spawner
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Advances the spawner in time
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        void Tick(double seconds);
    }
}