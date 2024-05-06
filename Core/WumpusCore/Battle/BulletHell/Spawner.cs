namespace WumpusCore.Battle.BulletHell
{
    public interface Spawner
    {
        int DisplayID { get; }
        
        Vector2 Position { get; }

        /// <summary>
        /// Advances the spawner in time
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        void Tick(double seconds);
        
        
    }
}