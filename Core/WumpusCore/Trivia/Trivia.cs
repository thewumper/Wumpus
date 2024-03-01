using System;

namespace WumpusCore.Trivia
{
    /// <summary>
    /// A minigame that asks a player a number of trivia questions.
    /// </summary>
    public class Trivia: Minigame
    {
        private Questions questions;
        // The question we're waiting on the player to answer
        private AnsweredQuestion currentQuestion;
        private int totalRoundQuestions;
        // If player gets this many or more questions right they win the round
        private int winThreshold;
        private int questionsAnswered;
        private int questionsWon;
        
        /// <summary>
        /// The number of questions that are available to ask.
        /// </summary>
        public int Count { get { return questions.Count; } }

        /// <summary>
        /// Initialize a Trivia object
        /// </summary>
        /// <param name="filepath">The path to the json file to read questions from</param>
        public Trivia(string filepath)
        {
            questions = new Questions(filepath);
        }

        /// <summary>
        /// Starts a new round of trivia. Will report to minigame controller when the player wins or loses
        /// </summary>
        /// <param name="roundQuestions">The total number of questions in the round</param>
        /// <param name="winThreshold">The number of questions the player must answer correctly in order to win the round</param>
        public void StartRound(int roundQuestions, int winThreshold)
        {
            totalRoundQuestions = roundQuestions;
            this.winThreshold = winThreshold;
            questionsAnswered = 0;
            questionsWon = 0;
            nextQuestion();
        }

        /// <summary>
        /// Get the question currently awaiting an answer
        /// </summary>
        /// <returns>The question currently awaiting an answer</returns>
        public AskableQuestion GetQuestion()
        {
            try
            {
                return currentQuestion.question;
            }
            catch (NullReferenceException e)
            {
                throw new InvalidOperationException("Question not initialized. Run nextQuestion()?");
            }
        }

        /// <summary>
        /// Submit an answer to the current question
        /// </summary>
        /// <param name="choice">The answer index selected</param>
        /// <returns>Whether the answer was correct</returns>
        public bool SubmitAnswer(int choice)
        {
            if (reportResult() != GameResult.InProgress)
            {
                throw new InvalidOperationException("Round is already over!");
            }
            
            if (questionsAnswered >= totalRoundQuestions)
            {
                throw new InvalidOperationException("There are no questions to answer!");
            }

            if (choice >= currentQuestion.choices.Length)
            {
                throw new ArgumentOutOfRangeException("Not a valid answer index");
            }
            
            
            // -1 reserved for refusal to answer
            if (choice < -1)
            {
                throw new ArgumentOutOfRangeException("Not a valid answer index");
            }
            
            questionsAnswered++;
            if (choice == currentQuestion.answer)
            {
                questionsWon++;
                nextQuestion();
                return true;
            }
            
            nextQuestion();
            return false;
        }

        // Sets the next question
        private void nextQuestion()
        {
            currentQuestion = questions.GetQuestion();
        }

        /// <summary>
        /// Returns whether the player won
        /// </summary>
        /// <returns>A GameResult denoting the status from the perspective of the player</returns>
        public GameResult reportResult()
        {
            if (questionsWon >= winThreshold)
            {
                return GameResult.Win;
            }

            if (totalRoundQuestions - questionsAnswered < winThreshold)
            {
                return GameResult.Loss;
            } 
            
            return GameResult.InProgress;
        }
        
        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Leaves the question in the stack.
        /// </summary>
        /// <returns>A question that hasn't yet been used</returns>
        public AnsweredQuestion PeekRandomQuestion()
        {
            return questions.PeekRandomQuestion();
        }
    }
}