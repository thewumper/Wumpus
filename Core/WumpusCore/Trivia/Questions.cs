using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WumpusCore.Trivia
{
    /// <summary>
    /// A store of multiple choice questions for use by trivia.
    /// </summary>
    public class Questions
    {
        // All questions that could still be asked of the player
        private Stack<AnsweredQuestion> remainingQuestions;

        /// <summary>
        /// Pulls all questions from a specified file and overwrites the question list with the new questions, randomly ordered.
        /// Questions should be stored in JSON format.
        /// An array of objects in the format {"questions":"", "choices":["","",""...], "correct":0}
        /// </summary>
        /// <param name="filePath">The path to the file to read questions from</param>
        public Questions(string filePath)
        {
            JArray questionsArray = JArray.Parse(File.ReadAllText(filePath));

            for (int i = questionsArray.Count - 1; i >= 0; i--)
            {
                int index = Controller.Controller.Random.Next(i);
                
                JToken question = questionsArray[index];

                string text = question.SelectToken("text").Value<string>();
                string[] choices = question.SelectToken("choices").Value<string[]>();
                int answer = question.SelectToken("answer").Value<int>();
                
                remainingQuestions.Append(new AnsweredQuestion(text, choices, answer));
                questionsArray.RemoveAt(index);
            }
        } 
        
        // Gets the index of a random question that has not yet been answered.
        private int getRandomQuestionIndex()
        {
            return Controller.Controller.Random.Next(remainingQuestions.Count);
        }
        
        /// <summary>
        /// Returns a random question that hasn't been read yet.
        /// Leaves the question in the stack.
        /// </summary>
        /// <returns>A question that hasn't yet been used</returns>
        public AnsweredQuestion PeekRandomQuestion()
        {
            return remainingQuestions.ElementAt(getRandomQuestionIndex());
        }

        /// <summary>
        /// Returns the next question on the question stack.
        /// Removes the question from queue.
        /// </summary>
        /// <returns>A question that hasn't yet been used</returns>
        public AnsweredQuestion GetQuestion()
        {
            AnsweredQuestion question = remainingQuestions.Pop();
            return question;
        }
    }
}