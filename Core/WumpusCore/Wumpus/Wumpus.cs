namespace WumpusCore.Wumpus
{
        /// <summary>
        /// state of wumpus.  
        /// Better description coming soon
        /// </summary>
        public enum State
        {
            Sleeping,
            FleeingArrow,
            FleeingCombat,
            Combat,
            Dead
        }
    internal class Wumpus
    {
        /// <summary>
        /// Current Room Wumpus is in.  
        /// Better description coming soon
        /// </summary>
        public int Position { get; private set; }
        private State WumpusState;
        /// <summary>
        /// Constructs the Wumpus.  
        /// Better description coming soon
        /// </summary>
        public Wumpus()
        {
            Position = 0;
            WumpusState = State.Sleeping;
        }
        /// <summary>
        /// Moves the wumpus into a connected room randomly a number of times depending on the state.  
        /// Better description coming soon
        /// </summary>
        /// <param name="Random">Represents a pseudo-random number generator, which is a device that produces a sequence of numbers that meet certain statistical requirements for randomness</param>
        /// <exception cref="NotImplementedException"></exception>
        public void move(Random Random)
        {
            throw new NotImplementedException();
            int maxMoves;
            if (WumpusState = 1)
            {
                maxMoves = 1;
            }
            else if (WumpusState = 2)
            {
                maxMoves = 4;
            }
                for (int i = 0; i<maxMoves; i++){
                if ((maxMoves==4&&i <= 1)||rand.Next(0,2)==1) {
                //code to move
                }
            }
            WumpusState = State.Sleeping;
        }
        /// <summary>
        /// Changes state and starts minigame, then changes state again based on result of battle.  
        /// Better description coming soon
        /// </summary>
        public void startBattle()
        {
            throw new NotImplementedException();

        }   
    }
}