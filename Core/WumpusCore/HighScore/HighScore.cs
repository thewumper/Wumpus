using System;
using System.Runtime.InteropServices;
using WumpusCore.Topology;

namespace WumpusCore.HighScoreNS
{
    internal class HighScore
    {
        /// <summary>
        /// name of the player inputted at game beginning or end
        /// </summary>
        private string playerName;

        /// <summary>
        /// once the score is calculated it is stored here
        /// </summary>
        private int score;

        /// <summary>
        /// the number of turns the player took to win
        /// </summary>
        private int numTurns;

        /// <summary>
        /// Gold coins still in player's inventory
        /// </summary>
        private int goldLeft;

        /// <summary>
        /// Arrows still in player's inventory
        /// </summary>
        private int arrowsLeft;

        /// <summary>
        /// Did wumpus get deaded
        /// </summary>
        private bool isWumpusDead;

        /// <summary>
        /// The map that was played from the round
        /// </summary>
        private int mapUsed;

        /// <summary>
        /// the greatest elevation of all
        /// the scores
        /// </summary>
        private int currentHighScore;

        /// <summary>
        /// The list of storedHighScore structs for the 10 highest scores
        /// </summary>
        private StoredHighScore[] top10HighScores;
        
        /// <summary>
        /// HighScore object is how the score of the game
        /// is calculated and stored to files
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="numTurns"></param>
        /// <param name="goldLeft"></param>
        /// <param name="arrowsLeft"></param>
        /// <param name="isWumpusDead"></param>
        /// <param name="mapUsed"></param>
        public HighScore(string playerName, int numTurns, int goldLeft, int arrowsLeft, bool isWumpusDead, int mapUsed)
        {
            this.playerName = playerName;
            this.numTurns = numTurns;
            this.goldLeft = goldLeft;
            this.arrowsLeft = arrowsLeft;
            this.isWumpusDead = isWumpusDead;
            this.mapUsed = mapUsed;

            this.score = calculateScore();

            StoredHighScore test = new StoredHighScore();
            test.
        }

        /// <summary>
        /// Score is calculated from the game variables, then stored
        /// in the score field
        /// </summary>
        /// <returns></returns>
        private int calculateScore()
        {
            int calculatedScore = 
                (100
                - this.numTurns
                + this.goldLeft
                + (5 * this.arrowsLeft)
                + (translateWumpusLife() * 50));
            return calculatedScore;
        }

        /// <summary>
        /// Turns the bool isWumpusDead into
        /// an int so the score can be calculated
        /// using the translated 1 or 0
        /// </summary>
        /// <param name="isWumpusDead"></param>
        /// <returns></returns>
        private int translateWumpusLife()
        {
            if (this.isWumpusDead)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// returns the score field that has been
        /// calculated from the other variables passed
        /// for the object
        /// </summary>
        /// <returns></returns>
        public int getScore() { return this.score; }

        private void compareHighScore()
        {
            this.currentHighScore = Math.Max(this.score, currentHighScore);
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
        struct StoredHighScore
        {
            public int score;
            public string playerName;
            public int numTurns;
            public int goldLeft;
            public int arrowsLeft;
            public bool isWumpusDead;
            public int mapUsed;

            public StoredHighScore(string playerName, int numTurns, int goldLeft, int arrowsLeft, bool isWumpusDead, int mapUsed)
            {
                
            }
            
        }
    }
}
