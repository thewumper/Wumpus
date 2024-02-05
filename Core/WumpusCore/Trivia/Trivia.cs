using System;

namespace WumpusCore.Trivia
{
    /// <summary>
    /// A minigame that asks a player a number of trivia questions.
    /// </summary>
    public class Trivia
    {
        // The question we're waiting on the player to answer
        private Question currentQuestion;
        private int totalRoundQuestions;
        // If player gets this many or more questions right they win the round
        private int winThreshold;
        private int questionsAnswered;
        private int questionsWon;

        /// <summary>
        /// Starts a new round of trivia. Will report to minigame controller when the player wins or loses
        /// </summary>
        /// <param name="roundQuestions">The total number of questions in the round</param>
        /// <param name="questionsNecessary">The number of questions the player must win</param>
        public void StartRound(int roundQuestions, int questionsNecessary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Submit an answer to the current question
        /// </summary>
        /// <param name="choice">The answer index selected</param>
        /// <returns></returns>
        public bool SubmitAnswer(int choice)
        {
            throw new NotImplementedException();
        }

        // Sets the next question
        private void nextQuestion()
        {
            throw new NotImplementedException();
        }

        // Reports to minigame controller whether the player won the round or not
        private void reportResult(bool winner)
        {
            throw new NotImplementedException();
        }
    }
}