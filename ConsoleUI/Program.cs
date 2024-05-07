using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Controller controller = new Controller("../../../Questions.json", "../../../", 0);
            ConsoleUI ui = new ConsoleUI();
            ui.StartGame(controller);
        }

    }
}