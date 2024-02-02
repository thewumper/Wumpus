using System;

namespace WumpusCore
{
    public class MinigameController
    {
        // private Trivia trivia;

        /// <summary>
        /// Handles running Minigames and talks to the trivia 
        /// </summary>
        public MinigameController()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the result of the minigame, won or lost.
        /// </summary>
        /// <param name="result">If the player has won (true) or lost (false).</param>
        public void SetGameResult(bool result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the trivia question.
        /// </summary>
        /// <returns>The trivia question as a string.</returns>
        public string GetTriviaQuestion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Submits the trivia answer to the Trivia.
        /// </summary>
        /// <param name="guess">The player's guess to what the answer is.</param>
        /// <returns>If the guess was correct or incorrect.</returns>
        public bool SubmitTriviaAnswer(int guess)
        {
            throw new NotImplementedException();
        }
    }
}
