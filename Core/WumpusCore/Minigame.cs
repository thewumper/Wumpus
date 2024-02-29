using WumpusCore.Trivia;

namespace WumpusCore
{
    public interface Minigame
    {
        /// <summary>
        /// Returns whether the player won
        /// </summary>
        /// <returns>True if won, False if lost, and null if game not over</returns>
        GameResult reportResult();
        
    }
}