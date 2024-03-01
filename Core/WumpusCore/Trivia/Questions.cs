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
        
        public int Count { get { return remainingQuestions.Count; } }

        private struct questionClass
        {
            public string question;
            public string[] choices;
            public int answer;
        }
        
        /// <summary>
        /// Pulls all questions from a specified file and overwrites the question list with the new questions, randomly ordered.
        /// Questions should be stored in JSON format.
        /// An array of objects in the format {"questions":"", "choices":["","",""...], "correct":0}
        /// </summary>
        /// <param name="filePath">The path to the file to read questions from</param>
        public Questions(string filePath)
        {
            remainingQuestions = new Stack<AnsweredQuestion>();
            
            JArray questionsArray = JArray.Parse(File.ReadAllText(filePath));

            for (int i = questionsArray.Count - 1; i >= 0; i--)
            {
                int index = Controller.Controller.Random.Next(i);
                
                JToken question = questionsArray[index];

                questionClass tempQuestion = JsonConvert.DeserializeObject<questionClass>(question.ToString());

                AnsweredQuestion appendQuestion =
                    new AnsweredQuestion(tempQuestion.question, tempQuestion.choices, tempQuestion.answer);
                
                remainingQuestions.Push(appendQuestion);
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
            if (remainingQuestions.Count == 0)
            {
                throw new InvalidOperationException("No questions to peek.");
            }
            
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