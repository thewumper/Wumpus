using System;
using System.Runtime.InteropServices;
using WumpusCore.Topology;

namespace WumpusCore.HighScoreNS
{
    internal class HighScore
    {
        /// <summary>
        /// The list of storedHighScore structs for the 10 highest scores
        /// </summary>
        private StoredHighScore[] top10HighScores;

        /// <summary>
        /// HighScore object is how the score of the game is calculated and stored to files
        /// </summary>
        /// <param name="playerName"> Name of player who owns the score </param>
        /// <param name="numTurns"> How long it took for game to end in turns </param>
        /// <param name="goldLeft"> Number of gold remaining when game ends </param>
        /// <param name="arrowsLeft"> Number of arrows remaining when game ends </param>
        /// <param name="isWumpusDead"> Was the wumpus killed when the game ended </param>
        /// <param name="mapUsed"> Number code of the map generation from the game </param>
        public HighScore(string playerName, int numTurns, int goldLeft, int arrowsLeft, bool isWumpusDead, int mapUsed)
        {
            StoredHighScore compactScore = new StoredHighScore(
                this.calculateScore(numTurns, goldLeft, arrowsLeft, isWumpusDead),
                playerName, numTurns, goldLeft, arrowsLeft, isWumpusDead, mapUsed);
        }

        /// <summary>
        /// Score is calculated from the game variables, then stored in the score field
        /// </summary>
        /// <param name="numTurns"> Length of game in turns </param>
        /// <param name="goldLeft"> Gold at game end </param>
        /// <param name="arrowsLeft"> Arrows at game end </param>
        /// <param name="isWumpusDead"> Wumpus was killed at game end </param>
        /// <returns> The score of the game based on the parameters </returns>
        private int calculateScore(int numTurns, int goldLeft, int arrowsLeft, bool isWumpusDead)
        {
            int calculatedScore = 
                (100
                - numTurns
                + goldLeft
                + (5 * arrowsLeft)
                + (translateWumpusLife(isWumpusDead) * 50));
            return calculatedScore;
        }

        /// <summary>
        /// Turns the bool isWumpusDead into
        /// an int so the score can be calculated
        /// using the translated 1 or 0
        /// </summary>
        /// <param name="isWumpusDead"> Was the wumpus killed </param>
        /// <returns> Translated bool into 1 or 0 for calculateScore() </returns>
        private int translateWumpusLife(bool isWumpusDead)
        {
            if (isWumpusDead)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Stores an individual HighScore struct to the file
        /// </summary>
        private void storeScoreToFile()
        {
            // file things
        }

        /// <summary>
        /// Loops through the high score struct list and
        /// stores them on the file
        /// </summary>
        private void storeTopTenToFile()
        {
            // multiple file things
            // call storeScoreToFile() probably
        }

        /// <summary>
        /// Compacted score, stores the score
        /// along with the player name and the
        /// variables that allowed the score to
        /// be achieved
        /// </summary>
        public struct StoredHighScore
        {
            public readonly int score;
            public readonly string playerName;
            public readonly int numTurns;
            public readonly int goldLeft;
            public readonly int arrowsLeft;
            public readonly bool isWumpusDead;
            public readonly int mapUsed;

            public StoredHighScore(int score, string playerName, int numTurns, int goldLeft, int arrowsLeft, bool isWumpusDead, int mapUsed)
            {
                this.playerName = playerName;
                this.numTurns = numTurns;
                this.goldLeft = goldLeft;
                this.arrowsLeft = arrowsLeft;
                this.isWumpusDead = isWumpusDead;
                this.mapUsed = mapUsed;

                this.score = score;
            }
            
        }
    }
}
