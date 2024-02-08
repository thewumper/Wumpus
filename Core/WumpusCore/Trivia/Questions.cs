using System;
using System.Collections.Generic;
using System.Linq;


namespace WumpusCore.Trivia
{
    /// <summary>
    /// A store of multiple choice questions for use by trivia.
    /// </summary>
    public class Questions
    {
        // All questions that could still be asked of the player
        private LinkedList<AnsweredQuestion> remainingQuestions;

        /// <summary>
        /// Pulls all questions from a specified file and overwrites the question list with the new questions.
        /// Questions should be stored in JSON format.
        /// An array of objects in the format {"questions":"", "choices":["","",""...], "correct":0}
        /// </summary>
        /// <param name="filePath">The path to the file to read questions from</param>
        /// <exception cref="NotImplementedException"></exception>
        public Questions(string filePath)
        {
            throw new NotImplementedException("Need to import Newtonsoft.Json first!"); 
        } 
        
        // Gets the index of a random question that has not yet been answered.
        private int getRandomQuestionIndex()
        {
            return Controller.Controller.Random.Next(remainingQuestions.Count);
        }
        
        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Leaves the question in queue.
        /// </summary>
        /// <returns>A question that hasn't yet been used</returns>
        public AnsweredQuestion PeekRandomQuestion()
        {
            return remainingQuestions.ElementAt(getRandomQuestionIndex());
        }

        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Removes the question from queue.
        /// </summary>
        /// <returns>A question that hasn't yet been used</returns>
        public AnsweredQuestion GetQuestion()
        {
            int index = getRandomQuestionIndex();
            AnsweredQuestion question = remainingQuestions.ElementAt(index);
            remainingQuestions.Remove(question);
            return question;
        }
    }
}