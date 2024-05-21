namespace WumpusCore.Battle
{
    public interface IBattleTypeController
    {
        /// <summary>
        /// The type of battle this is the state of
        /// </summary>
        BattleType type { get; }
    }
}