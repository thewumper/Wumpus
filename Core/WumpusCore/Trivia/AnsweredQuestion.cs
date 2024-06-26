using System;

namespace WumpusCore.Trivia
{
    /// <summary>
    /// A multiple choice question, containing question text, answer options, and the correct answer.
    /// </summary>
    public class AnsweredQuestion
    {
        public AnsweredQuestion(string questionText, string[] choices, int answer)
        {
            this.question = new AskableQuestion(questionText, choices);
            this.answer = answer;
        }

        public String LogEntry()
        {
            return QuestionText + "\n" + choices[answer] + ".";
        }

        /// <summary>
        /// Everything but the answer. Safe to hand out of Trivia.
        /// </summary>
        public readonly AskableQuestion question;
        
        /// <summary>
        /// The text that will be presented as a question to the player.
        /// </summary>
        public string QuestionText
        {
            get { return question.questionText; }
        }
        
        /// <summary>
        /// Each choice of possible answer.
        /// </summary>
        public string[] choices
        {
            get { return question.choices; }
        }
        
        /// <summary>
        /// The index of the choice that is correct.
        /// </summary>
        public readonly int answer;
    }
}