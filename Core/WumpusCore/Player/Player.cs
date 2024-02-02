using System;
using System.IO;
namespace WumpusCore.Player
{
    public class Player
    {
        public int position { get; private set; }

        private string sprPath = null;

        // Inventory
        public int coins { get; set; }
        public int arrows { get; set; }

        public Player()
        {
            coins = 0;
            arrows = 0;

            if (!File.Exists(sprPath))
            {
                throw new FileNotFoundException();
            }
        }
    }
}