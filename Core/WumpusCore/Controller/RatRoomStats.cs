namespace WumpusCore.Controller
{
    /// <summary>
    /// A wrapper for the stats the UI might need from controller in a rat room
    /// Includes a time in room, starting coins, remaining coins, and damage dealt.
    /// This object is not meant to be reused, the values don't update when gotten.
    /// </summary>
    public struct RatRoomStats
    {
        public RatRoomStats(int timeInRoom, int startingCoins, int remainingCoins, int damageDelt)
        {
            this.TimeInRoom = timeInRoom;
            this.StartingCoins = startingCoins;
            this.RemainingCoins = remainingCoins;
            this.DamageDelt = damageDelt;
        }

        /// <summary>
        /// The time you've spent in the curret room
        /// </summary>
        public int TimeInRoom { get; }
        /// <summary>
        /// The number of coins you started with when you entered the room
        /// </summary>
        public int StartingCoins { get; }
        /// <summary>
        /// The damage (in coins) that the rats have done to you
        /// </summary>
        public int DamageDelt { get; }
        /// <summary>
        /// The coins you have left after the amount of rat damage has been done
        /// </summary>
        public int RemainingCoins { get; }
    }
}