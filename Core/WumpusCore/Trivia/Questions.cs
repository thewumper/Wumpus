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

            initQuestions(questionsArray);
        }

        public Questions(FileStream file)
        {
            remainingQuestions = new Stack<AnsweredQuestion>();

            JArray questionsArray = JArray.Parse(new StreamReader(file).ReadToEnd());

            initQuestions(questionsArray);
        }

        /// <summary>
        /// Pulls all questions from specified files and overwrites the question list with the new questions, randomly ordered.
        /// Questions should be stored in JSON format.
        /// An array of objects in the format {"questions":"", "choices":["","",""...], "correct":0}
        /// </summary>
        /// <param name="filePaths">An array of paths to the files to read questions from</param>
        public Questions(string[] filePaths)
        {
            remainingQuestions = new Stack<AnsweredQuestion>();
            
            JArray questionsArray = new JArray();

            foreach (string filePath in filePaths)
            {
                questionsArray.Append(JArray.Parse(File.ReadAllText(filePath)));
            }

            initQuestions(questionsArray);
        }

        private void initQuestions(JArray questionsArray)
        {

            List<AnsweredQuestion> questions = new List<AnsweredQuestion>();
            foreach (JToken token in questionsArray)
            {
                questionClass tempQuestion = JsonConvert.DeserializeObject<questionClass>(token.ToString());
                questions.Add(new AnsweredQuestion(tempQuestion.question, tempQuestion.choices, tempQuestion.answer));
            }

            // https://stackoverflow.com/questions/273313/randomize-a-listt
            int n = questions.Count;
            while (n > 1) {
                n--;
                int k = Controller.Controller.Random.Next(n + 1);
                (questions[k], questions[n]) = (questions[n], questions[k]);
            }

            foreach (AnsweredQuestion question in questions)
            {
                remainingQuestions.Push(question);
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