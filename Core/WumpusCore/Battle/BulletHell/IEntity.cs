namespace WumpusCore.Battle.BulletHell
{
    public interface IEntity: IDisplayable
    {
        /// <summary>
        /// The object's position in space
        /// </summary>
        Vector2 Position { get; }
        
        /// <summary>
        /// The direction the object is moving in space
        /// </summary>
        Vector2 Velocity { get; }

        /// <summary>
        /// The direction the object is curving in its motion
        /// </summary>
        Vector2 Acceleration { get; }
    }
}