using WumpusCore.Controller;

namespace ConsoleUI
{
    public class ConsoleUI
    {
        public static void Main(string[] args)
        {
            Controller controller = new Controller("../../../../Unity/Assets/Trivia/Questions.json", "../../../../Unity/Assets/Maps", 0);
            Console.WriteLine(controller.GetState());
        }
    }
}