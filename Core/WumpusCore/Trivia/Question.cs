using System;

namespace WumpusCore.Trivia
{
    /// <summary>
    /// A multiple choice question
    /// </summary>
    public struct Question
    {
        /// <summary>
        /// The text that will be presented as a question to the player
        /// </summary>
        public readonly string text;
        
        /// <summary>
        /// Each choice of possible answer
        /// </summary>
        public readonly string[] choices;
        
        /// <summary>
        /// The index of the choice that is correct
        /// </summary>
        public readonly int correctChoice;
    }
}