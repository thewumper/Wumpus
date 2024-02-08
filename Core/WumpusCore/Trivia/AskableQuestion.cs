namespace WumpusCore.Trivia
{
    public struct AskableQuestion
    {
        /// <summary>
        /// A multiple choice question, containing question text and answer options. Does not reveal correct answer.
        /// Safe to pass outside of Trivia.
        /// </summary>
        public AskableQuestion(string text, string[] choices)
        {
            this.text = text;
            this.choices = choices;
        }

        /// <summary>
        /// The text that will be presented as a question to the player.
        /// </summary>
        public readonly string text;
        
        /// <summary>
        /// Each choice of possible answer.
        /// </summary>
        public readonly string[] choices;
    }
}