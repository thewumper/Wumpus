namespace WumpusCore.Battle.BulletHell
{
    public interface ISpawner: IEntity
    {
        /// <summary>
        /// Advances the spawner in time
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        void Tick(double seconds);
    }
}