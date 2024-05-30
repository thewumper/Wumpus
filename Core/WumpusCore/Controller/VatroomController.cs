namespace WumpusCore.Controller
{
    public class VatroomController : ISubcontroller
    {
        public bool isTriviaRunning { get; private set; }

        public bool CanExitRoom()
        {
            if (isTriviaRunning)
            {
                return false;
            }

            return true;
        }
    }

    public interface ISubcontroller
    {
        bool CanExitRoom();
    }
}