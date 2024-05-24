namespace WumpusCore.Battle.BulletHell
{
    public interface IDisplayable
    {
        /// <summary>
        /// Represents the sprite of the object, or any effects, or anything else. Interpreted by and assigned by a smarter part of the code than this
        /// </summary>
        int DisplayID { get; }
    }
}