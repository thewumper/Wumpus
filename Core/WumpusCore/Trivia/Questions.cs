using System;
using System.Collections.Generic;

namespace WumpusCore.Trivia
{
    /// <summary>
    /// A store of multiple choice questions for use by trivia.
    /// </summary>
    public class Questions
    {
        // All questions that could still be asked of the player
        private LinkedList<Question> remainingQuestions;

        /// <summary>
        /// Pulls all questions from a specified file and overwrites the question list with the new questions.
        /// Questions should be stored in JSON format.
        /// An array of objects in the format {"questions":"", "choices":["","",""...], "correct":0}
        /// </summary>
        /// <param name="filePath">The path to the file to read questions from</param>
        /// <exception cref="NotImplementedException"></exception>
        public void InitializeQuestions(string filePath)
        {
            throw new NotImplementedException();
        }
        
        // Gets the index of a random question that has not yet been answered.
        private Question getRandomQuestionIndex()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Leaves the question in queue.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Question peekRandomQuestion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Removes the question from queue.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Question getQuestion()
        {
            throw new NotImplementedException();
        }
    }
}