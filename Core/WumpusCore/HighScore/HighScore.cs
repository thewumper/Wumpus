using System;
using System.Collections.Generic;

namespace WumpusCore.HighScoreNS
{
    internal class HighScore
    {
        /// <summary>
        /// The list of storedHighScore structs for the 10 highest scores
        /// </summary>
        private List<StoredHighScore> topTenHighScores;

        /// <summary>
        /// The StoredHighScore struct made when the HighScore object is constructed
        /// </summary>
        public readonly StoredHighScore compactScore;

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
            this.compactScore = compactScore;


            checkTopTen();
            reorganizeTopTen();
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
        /// Makes sure the topTenHighScore list is created,
        /// and adds the StoredHighScores into the list if
        /// it has a top score
        /// </summary>
        private void checkTopTen()
        {
            if (topTenHighScores == null)
            {
                topTenHighScores = new List<StoredHighScore>();
            }

            int scoreLength = topTenHighScores.Count;
            if (scoreLength < 10)
            {
                topTenHighScores.Insert(scoreLength, compactScore);
                return;
            }
            
            for (int i = 0; i < scoreLength; i++)
            {
                if (topTenHighScores[i].score < compactScore.score)
                {
                    topTenHighScores.Insert(i, compactScore);
                    topTenHighScores.RemoveAt(10);
                    break;
                }
            }
        }

        /// <summary>
        /// Sorts the ten high scores by score (highest to lowest)
        /// </summary>
        private void reorganizeTopTen()
        {
            topTenHighScores.Sort((score1, score2)=>
            {
                return score1.score.CompareTo(score2.score);
            });
        }

        private StoredHighScore convertStringToStoredHighScore(string compScore)
        {
            string[] variables = compScore.Split(',');
            string player = variables[0].Substring(0, variables[0].IndexOf(':'));
            int score = int.Parse(variables[0].Substring(variables[0].IndexOf(':') + 1));
            int turns = int.Parse(variables[1].Substring(variables[1].IndexOf(':') + 1));
            int gold = int.Parse(variables[2].Substring(variables[2].IndexOf(':') + 1));
            int arrows = int.Parse(variables[3].Substring(variables[3].IndexOf(':') + 1));
            bool wumpusDead = bool.Parse(variables[4].Substring(variables[4].IndexOf(':') + 1));
            int mapUsed = int.Parse(variables[5].Substring(variables[5].IndexOf(':') + 1, variables[5].Length - variables[5].IndexOf(']') + 1));

            StoredHighScore boxScore = new StoredHighScore(score, player, turns, gold, arrows, wumpusDead, mapUsed);
            return boxScore;
        }

        private void seperateFile(string file)
        {
            string topTenHighScores = file.Substring(0, file.IndexOf("Personal Score"));
            Console.WriteLine(topTenHighScores);
            string[] scores = topTenHighScores.Split('[');
            for (int i = 0; i < scores.Length; i++)
            {
                Console.WriteLine(scores[i]);
            }
        }

        /// <summary>
        /// Stores an individual StoredHighScore struct to the file
        /// </summary>
        /// <param name="compScore"> Struct to access and save </param>
        public void storeScoreToFile(StoredHighScore compScore)
        {
            string saveScore = compScore.ToString();

            SaveFile saveFile = new SaveFile(saveScore);
        }

        /// <summary>
        /// Loops through the high score struct list and
        /// stores them on the file
        /// </summary>
        public void storeTopTenToFile()
        {
            if (topTenHighScores == null)
            {
                throw new Exception("TopTenHighScores is null");
            }

            string allTextToSave = "Top Ten Scores: \n";
            
            for (int i = 0; i < topTenHighScores.Count; i++)
            {
                string infoToSave = topTenHighScores[i].ToString();
                allTextToSave += infoToSave + "\n\n";

                convertStringToStoredHighScore(infoToSave);
            }

            allTextToSave += "Personal Score: \n" + compactScore.ToString();

            SaveFile saveFile = new SaveFile(allTextToSave);
            string text = saveFile.ReadFile(false);
            seperateFile(text);

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
            
            public override string ToString()
            {
                return "[" + this.playerName + ": "
                + this.score.ToString() + ",\n Turns: "
                + this.numTurns.ToString() + ",\n Gold remaining: "
                + this.goldLeft.ToString() + ", \n Arrows remaining: "
                + this.arrowsLeft.ToString() + ", \n Wumpus slain: "
                + this.isWumpusDead.ToString() + ", \n Map played: "
                + this.mapUsed.ToString() + "]";
            }
        }
    }
}
