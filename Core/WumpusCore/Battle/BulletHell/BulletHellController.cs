using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace WumpusCore.Battle.BulletHell
{
    public class BulletHellController
    {
        /// <summary>
        /// All currently present bullets
        /// </summary>
        public List<Bullet> Bullets;

        private Stopwatch previousTick;

        public BulletHellController()
        {
            Bullets = new List<Bullet>();
            previousTick = new Stopwatch();
            previousTick.Start();
        }

        /// <summary>
        /// Advances the game in time
        /// Should be called before bullets are instantiated
        /// </summary>
        /// <param name="seconds">The number of seconds since the last game tick</param>
        public void Tick(double seconds)
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Tick(seconds);
            }
        }

        /// <summary>
        /// Advances the game in time, based on how long it has been since the previous tick.
        /// </summary>
        public void Tick()
        {
            double seconds = previousTick.Elapsed.Ticks / 10000000.0;
            previousTick.Restart();
            Tick(seconds);
        }
    }
}