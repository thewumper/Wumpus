using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Trivia;

namespace WumpusTesting
{
    [TestClass]
    public class TriviaTesting
    {
        public Trivia makeTrivia()
        {
            string path = Path.GetFullPath("./Questions.json");
            Trivia trivia = new Trivia(path);
            if (trivia.Count == 0)
            {
                throw new Exception("THERE ARE NO QUESTIONS HERE!");
            }
            return trivia;
        }
        
        [TestMethod]
        public void GrabQuestions()
        {
            Trivia trivia = makeTrivia();
            for (int i = 0; i < 100; i++)
            {
                AnsweredQuestion question = trivia.PeekRandomQuestion();
                AnsweredQuestion question2 = trivia.PeekRandomQuestion();
                if (question != question2)
                {
                    return;
                }
            }

            Assert.Fail();
        }

        [TestMethod]
        public void MakeQuestion()
        {
            string[] strings = new[] { "string1", "string2" };
            AnsweredQuestion question = new AnsweredQuestion("question", strings, 1);
        }

        [TestMethod]
        public void TriviaRound()
        {
            Trivia trivia = makeTrivia();
            trivia.StartRound(5, 3);
            trivia.GetQuestion();
            Assert.AreEqual(GameResult.InProgress, trivia.reportResult());
            Assert.IsFalse(trivia.SubmitAnswer(-1));
            Assert.AreEqual(GameResult.InProgress, trivia.reportResult());
            Assert.IsFalse(trivia.SubmitAnswer(-1));
            Assert.AreEqual(GameResult.InProgress, trivia.reportResult());
            Assert.IsFalse(trivia.SubmitAnswer(-1));
            Assert.AreEqual(GameResult.Loss, trivia.reportResult());
            Action submit = () => { trivia.SubmitAnswer(-1); };
            Assert.ThrowsException<InvalidOperationException>(submit);
        }
    }
}